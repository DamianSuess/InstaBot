using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Console.Settings
{
    public class AuthSettings : BaseSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Guid { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public IEnumerable<Cookie> Cookies { get; set; }
    }
}
