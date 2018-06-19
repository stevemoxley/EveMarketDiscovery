using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveAccountant
{
    public static class AuthenticationManager
    {
        public static AccessToken AccessToken
        {
            get
            {
                if(accessToken == null)
                {
                    if(File.Exists(Path.Combine(authDirectory, accessTokenFile)))
                    {
                        var fileJson = File.ReadAllText(Path.Combine(authDirectory, accessTokenFile));
                        var accessToken = JsonConvert.DeserializeObject<AccessToken>(fileJson);
                        return accessToken;
                    }
                    return null;
                }
                return accessToken;
            }
            set
            {
                accessToken = value;
                if(!Directory.Exists(authDirectory))
                {
                    Directory.CreateDirectory(authDirectory);
                }
                File.WriteAllText(Path.Combine(authDirectory, accessTokenFile), JsonConvert.SerializeObject(accessToken));
            }
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


        private static AccessToken accessToken;
        private static CharacterInfo characterInfo;

        private static string authDirectory = "cache/auth";

        private static string accessTokenFile = "auth.txt";
        private static string characterIdFile = "char.txt";
    }
}
