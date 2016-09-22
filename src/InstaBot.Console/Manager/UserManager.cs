using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InstaBot.Console.Model;
using InstaBot.Console.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstaBot.Console.Manager
{
    public interface IUserManager
    {
        Task<LoginResponseMessage> Login();
        Task<bool> SyncFeatures();
    }

    public class UserManager : BaseManager, IUserManager
    {
        private const string GetInfo = "users/{userId}/info/";
        private const string GetHeader = "si/fetch_headers/?challenge_type=signup&guid={0}";
        private const string PostLogin = "accounts/login/";
        private const string GetSync = "qe/sync/";

        public UserManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }

        public async Task<LoginResponseMessage> Login()
        {
            var response = await WebApi.InnerClient.GetAsync(string.Format(GetHeader, Guid.NewGuid().ToString("N")));

            if (!response.IsSuccessStatusCode)
                throw new IntagramException("Couldn't get challenge, check your connection");

            var csrfToken = ExtractToken(response);
            if(string.IsNullOrWhiteSpace(csrfToken)) throw new IntagramException("Missing csfrtoken");

            var message =
                new LoginMessage(csrfToken, ConfigurationManager.AuthSettings.Login,
                    ConfigurationManager.AuthSettings.Password);

            var content = SignedContent(message.ToString());

            var loginResponse = await WebApi.InnerClient.PostAsync(PostLogin, content);
            if (!loginResponse.IsSuccessStatusCode) throw new IntagramException(loginResponse.ReasonPhrase);

            var user= JsonConvert.DeserializeObject<LoginResponseMessage>(loginResponse.Content.ReadAsStringAsync().Result);
            if (user.Status.Equals("ok"))
            {
                var token = ExtractToken(loginResponse);
                if (string.IsNullOrWhiteSpace(csrfToken)) throw new IntagramException("Missing csfrtoken");

                ConfigurationManager.AuthSettings.Guid = Guid.NewGuid().ToString();
                ConfigurationManager.AuthSettings.Token = token;
                ConfigurationManager.AuthSettings.UserId = user.LoggedInUser.Id;
                ConfigurationManager.AuthSettings.Save();

                return user;
            }
            return null;
        }

        public async Task<bool> SyncFeatures()
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = ConfigurationManager.AuthSettings.Guid;
            syncMessage._uid = ConfigurationManager.AuthSettings.UserId;
            syncMessage._csrftoken = ConfigurationManager.AuthSettings.Token;
            syncMessage.id = ConfigurationManager.AuthSettings.UserId;
            syncMessage.experiments = ConfigurationManager.ApiSettings.Experiments;

            var content = SignedContent(syncMessage.ToString());

            var syncEntity = await WebApi.PostEntityAsync<SyncResponseMessage>(GetSync, content);
            return true;
        }

        private string ExtractToken(HttpResponseMessage response)
        {
            var cookieContainer = new CookieContainer();
            IEnumerable<string> cookies;
            if (response.Headers.TryGetValues("set-cookie", out cookies))
            {
                foreach (var c in cookies)
                {
                    cookieContainer.SetCookies(new Uri(ConfigurationManager.ApiSettings.Url), c);
                }
            }
            var csrfToken = string.Empty;
            var regex = new Regex("^csrftoken=(?<token>[^;]+); expires=(?<expire>[^;]+);", RegexOptions.IgnoreCase);
            if (!cookies.Any(x =>
            {
                var match = regex.Match(x);
                if (match.Success && match.Groups["token"].Success)
                {
                    csrfToken = match.Groups["token"].Value;
                    return match.Success;
                }
                return false;
            })) return null;
            return csrfToken;
        }
    }

}