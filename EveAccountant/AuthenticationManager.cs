using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveAccountant
{
    public static class AuthenticationManager
    {
        public static AccessToken AccessToken { get; set; }

        public static CharacterInfo CharacterInfo { get; set; }
    }
}
