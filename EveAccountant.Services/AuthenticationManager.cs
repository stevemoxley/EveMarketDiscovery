using DataAccess;
using EveSSO.Wallet.Journal;
using EveSSO.Wallet.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EveAccountant.Services
{
    public static class AuthenticationManager
    {
        public static AuthTokens AuthTokens
        {
            get
            {
                return GetAuthTokens();
            }
            set
            {
                authTokens = value;
                SaveAuthTokens(value);
            }
        }

        public static bool IsAuthorized()
        {
            return CharacterInfo != null && !string.IsNullOrEmpty(CharacterInfo.CharacterName);
        }

        public static string GetAuthenticationUrl()
        {
            return GetAuthenticationUrl(callbackUrl, clientId, scopes, "");
        }

        public static int LoadTransactionsAndJournals()
        {
            var journalEntries = JournalProvider.GetCharacterJournal(CharacterInfo.CharacterID, AuthTokens.access_token);
            var transactions = TransactionsProvider.GetCharacterTransactions(CharacterInfo.CharacterID, AuthTokens.access_token);
            var sellTransactions = transactions.Where(t => !t.is_buy).ToArray();
            var buyTransactions = transactions.Where(t => t.is_buy).ToArray();

            int result = 0;
            result += SaveAllTransactions(transactions);
            result += SaveAllJournalEntries(journalEntries);

            return result;
        }

        private static int SaveAllJournalEntries(JournalEntry[] journalEntries)
        {
            var journalEntryDAO = new JournalEntryDAO();
            int result = 0;
            foreach (var journalEntry in journalEntries)
            {
                var existingJournalEntry = journalEntryDAO.GetJournalEntry(journalEntry.id);
                if (existingJournalEntry == null)
                {
                    journalEntryDAO.Add(journalEntry);
                    result++;
                }
            }

            return result;
        }

        private static int SaveAllTransactions(EveSSO.Wallet.Transactions.Transaction[] transactions)
        {
            int result = 0;
            var transactionDAO = new TransactionDAO();
            foreach (var transaction in transactions)
            {
                //See if it exists
                var existingTransaction = transactionDAO.GetTransaction(transaction.transaction_id);
                if (existingTransaction == null)
                {
                    transactionDAO.Add(transaction);
                    result++;
                }
            }
            return result;
        }

        private static string GetAuthenticationUrl(string redirectUri, string clientId, string[] scopes, string state)
        {
            string scopesString = string.Join(" ", scopes);
            var encodedUri = WebUtility.UrlEncode(redirectUri);
            var encodedScopes = WebUtility.UrlEncode(scopesString);
            string url = $"https://login.eveonline.com/oauth/authorize/?response_type=code&redirect_uri={ encodedUri }&client_id={ clientId }&scope={ encodedScopes }&state={ state }";


            return url;
        }

        public static async Task<string> PostAuthCheck(string code)
        {
            var httpClient = new HttpClient();
            string authHeader = Base64Encode($"{ clientId }:{ secretKey }");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
            httpClient.DefaultRequestHeaders.Host = "login.eveonline.com";
            var values = new Dictionary<string, string>
            {
                { "grant_type","authorization_code" },
                { "code", code}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await httpClient.PostAsync("https://login.eveonline.com/oauth/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var authTokens = JsonConvert.DeserializeObject<AuthTokens>(responseString);
            authTokens.expiration_time = DateTime.Now + new TimeSpan(0, 0, authTokens.expires_in);
            AuthTokens = authTokens;
            CharacterInfo = await GetCharacterInformation(authTokens);

            return responseString;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static async Task<CharacterInfo> GetCharacterInformation(AuthTokens accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);
            client.DefaultRequestHeaders.Host = "login.eveonline.com";

            var charInfo = JsonConvert.DeserializeObject<CharacterInfo>(await client.GetStringAsync("https://login.eveonline.com/oauth/verify/"));

            return charInfo;
        }

        private static AuthTokens GetAuthTokensFromRefreshToken(string refreshToken)
        {
            try
            {
                var httpClient = new HttpClient();
                string authHeader = Base64Encode($"{ clientId }:{ secretKey }");

                string url = $"https://login.eveonline.com/oauth/token";
                var data = Encoding.ASCII.GetBytes($"grant_type=refresh_token&refresh_token={refreshToken}");


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                //request.Timeout = 3000;

                request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {authHeader}");

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    var authTokens = JsonConvert.DeserializeObject<AuthTokens>(result);
                    authTokens.expiration_time = DateTime.Now + new TimeSpan(0, 0, authTokens.expires_in);
                    AuthTokens = authTokens;
                    return authTokens;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static AuthTokens GetAuthTokens()
        {
            if (File.Exists(accessTokenFile))
            {
                var fileJson = File.ReadAllText(accessTokenFile);
                var authTokens = JsonConvert.DeserializeObject<AuthTokens>(fileJson);

                //Check if the accessToken is expired
                if (authTokens.expiration_time <= DateTime.Now)
                {
                    //Get the new token
                    authTokens = GetAuthTokensFromRefreshToken(authTokens.refresh_token);
                }
                return authTokens;
            }
            return null;
        }

        private static void SaveAuthTokens(AuthTokens authTokens)
        {
            File.WriteAllText(accessTokenFile, JsonConvert.SerializeObject(authTokens));
        }

        public static CharacterInfo CharacterInfo
        {
            get
            {
                if (characterInfo == null)
                {
                    if (File.Exists(characterIdFile))
                    {
                        var fileJson = File.ReadAllText(characterIdFile);
                        var characterInfo = JsonConvert.DeserializeObject<CharacterInfo>(fileJson);
                        return characterInfo;
                    }
                    return null;
                }
                return characterInfo;
            }
            set
            {
                characterInfo = value;
                File.WriteAllText(characterIdFile, JsonConvert.SerializeObject(characterInfo));
            }
        }

        private static AuthTokens authTokens;
        private static CharacterInfo characterInfo;

        private static string accessTokenFile = System.Web.HttpContext.Current.Server.MapPath("~/auth.txt");
        private static string characterIdFile = System.Web.HttpContext.Current.Server.MapPath("~/char.txt");

        private static string clientId = "b4d2612fa3bd497486e71d4a26465953";
        private static string secretKey = "hgg6KfFbexI3rIFJNHbmceGd0RuWMYQmJfcDLedQ";
        private static string[] scopes = { "esi-wallet.read_character_wallet.v1", "esi-markets.read_character_orders.v1" };
        private static string callbackUrl = "http://localhost:20039/SSO/PostAuth";
    }
}


public class AuthTokens
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public DateTime expiration_time { get; set; }
}


public class CharacterInfo
{
    public string CharacterID { get; set; }
    public string CharacterName { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string Scopes { get; set; }
    public string TokenType { get; set; }
    public string CharacterOwnerHash { get; set; }
}
