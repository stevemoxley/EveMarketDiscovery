using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EveAccountant
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
            if (File.Exists(Path.Combine(authDirectory, accessTokenFile)))
            {
                var fileJson = File.ReadAllText(Path.Combine(authDirectory, accessTokenFile));
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
            if (!Directory.Exists(authDirectory))
            {
                Directory.CreateDirectory(authDirectory);
            }
            File.WriteAllText(Path.Combine(authDirectory, accessTokenFile), JsonConvert.SerializeObject(authTokens));
        }

        public static CharacterInfo CharacterInfo
        {
            get
            {
                if (characterInfo == null)
                {
                    if (File.Exists(Path.Combine(authDirectory, characterIdFile)))
                    {
                        var fileJson = File.ReadAllText(Path.Combine(authDirectory, characterIdFile));
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
                File.WriteAllText(Path.Combine(authDirectory, characterIdFile), JsonConvert.SerializeObject(characterInfo));
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static AuthTokens authTokens;
        private static CharacterInfo characterInfo;

        private static string authDirectory = "cache/auth";

        private static string accessTokenFile = "auth.txt";
        private static string characterIdFile = "char.txt";

        private static string clientId = "b4d2612fa3bd497486e71d4a26465953";
        private static string secretKey = "hgg6KfFbexI3rIFJNHbmceGd0RuWMYQmJfcDLedQ";
    }
}
