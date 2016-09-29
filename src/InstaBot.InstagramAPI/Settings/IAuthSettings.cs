using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.InstagramAPI.Settings
{
    public interface IAuthSettings
    {
        string Login { get; set; }
        string Password { get; set; }
        string Guid { get; set; }
        string UserId { get; set; }
        string Token { get; set; }
        IEnumerable<Cookie> Cookies { get; set; }
        void Save();
    }
}
