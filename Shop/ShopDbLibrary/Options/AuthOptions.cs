using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDbLibrary.Options
{
    public static class AuthOptions
    {
        public static readonly string SecretKey = "12345678123456781234567812345678";
        public static readonly string Issuer = "myapp";
        public static readonly string Audience = "myapp-users";
    }
}
