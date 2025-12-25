using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDbLibrary.DTOs
{
    public class LoginReqest(string login, string password)
    {
        public string Login { get; set; } = login;
        public string Password { get; set; } = password;
    }
}
