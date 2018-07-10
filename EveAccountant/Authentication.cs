using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace EveAccountant
{
    public partial class Authentication : Form
    {
        public Authentication()
        {
            InitializeComponent();

            string[] scopes = { "esi-wallet.read_character_wallet.v1", "esi-markets.read_character_orders.v1" };

            webBrowser1.Navigate(GetAuthenticationUrl(callBackUrl, clientId, scopes, ""));
            webBrowser1.ScriptErrorsSuppressed = true;
        }          

        private void Authentication_Load(object sender, EventArgs e)
        {

        }

        private string GetAuthenticationUrl(string redirectUri, string clientId, string[] scopes, string state)
        {
            string scopesString = string.Join(" ", scopes);
            var encodedUri = WebUtility.UrlEncode(redirectUri);
            var encodedScopes = WebUtility.UrlEncode(scopesString);
            string url = $"https://login.eveonline.com/oauth/authorize/?response_type=code&redirect_uri={ encodedUri }&client_id={ clientId }&scope={ encodedScopes }&state={ state }";


            return url;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.AbsolutePath.Contains("/account/logon"))
            {

            }
            else if (e.Url.AbsoluteUri.Contains("/export/callback/"))
            {
                var redirectedUrl = e.Url.AbsoluteUri;
                var queryStringParameters = HttpUtility.ParseQueryString(e.Url.Query);
                var code = queryStringParameters[0];

                PostAuthCheck(code);
            }
        }

        private async void PostAuthCheck(string code)
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
            AuthenticationManager.AuthTokens = authTokens;
            GetCharacterInformation(authTokens);
            this.Close();
            MessageBox.Show("Authentication Successful!");
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private async void GetCharacterInformation(AuthTokens accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);
            client.DefaultRequestHeaders.Host = "esi.tech.ccp.is";

            var charInfo = JsonConvert.DeserializeObject<CharacterInfo>(await client.GetStringAsync("https://esi.tech.ccp.is/verify/"));

            AuthenticationManager.CharacterInfo = charInfo;
        }

        private string callBackUrl = "http://localhost/export/callback/";
        private string clientId = "b4d2612fa3bd497486e71d4a26465953";
        private string secretKey = "hgg6KfFbexI3rIFJNHbmceGd0RuWMYQmJfcDLedQ";

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


}
