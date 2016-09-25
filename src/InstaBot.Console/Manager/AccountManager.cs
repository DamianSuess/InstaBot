using System;
using System.Collections.Generic;
using System.Data;
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
        Task<LoginResponseMessage> Login(bool force = false);
        Task<bool> Logout();
        Task<bool> SyncFeatures();
        Task<AutoCompleteUserListResponseMessage> AutoCompleteUser();
        Task<TimelineFeedResponseMessage> TimeLineFeed();
    }

    public class AccountManager : BaseManager, IUserManager
    {
        private const string GetInfo = "users/{userId}/info/";
        private const string GetHeader = "si/fetch_headers/?challenge_type=signup&guid={0}";
        private const string PostLogin = "accounts/login/";
        private const string PostLogout = "accounts/logout/";
        private const string GetSync = "qe/sync/";
        private const string GetAutoCompleteUser = "friendships/autocomplete_user_list/?version=2";
        private const string GetTimelineFeed = "feed/timeline/";

        public AccountManager(ConfigurationManager configurationManager, IDbConnection session) : base(configurationManager)
        {
        }

        public async Task<LoginResponseMessage> Login(bool force = false)
        {
            if (!force && !string.IsNullOrWhiteSpace(ConfigurationManager.AuthSettings.UserId) &&
                !string.IsNullOrWhiteSpace(ConfigurationManager.AuthSettings.Token))
            {
                try
                {
                    var timeline = await TimeLineFeed();
                    if (!string.IsNullOrWhiteSpace(timeline.Message) && timeline.Message.Equals("login_required"))
                        await Login(true);
                }
                catch (InstagramException ex)
                {
                    return await Login(true);
                }

                return null;
            }
            var response = await WebApi.InnerClient.GetAsync(string.Format(GetHeader, Guid.NewGuid().ToString("N")));

            if (!response.IsSuccessStatusCode)
                throw new InstagramException("Couldn't get challenge, check your connection"); ;
            var csrfToken = ExtractToken(response);
            if (string.IsNullOrWhiteSpace(csrfToken)) throw new InstagramException("Missing csfrtoken");

            var message =
                new LoginMessage(csrfToken, ConfigurationManager.AuthSettings.Login,
                    ConfigurationManager.AuthSettings.Password);

            var content = SignedContent(message.ToString());

            var loginResponse = await WebApi.InnerClient.PostAsync(PostLogin, content);
            if (!loginResponse.IsSuccessStatusCode) throw new InstagramException(loginResponse.ReasonPhrase);

            var user =
                JsonConvert.DeserializeObject<LoginResponseMessage>(loginResponse.Content.ReadAsStringAsync().Result);
            if (user.Status.Equals("ok"))
            {
                var reponseCookie =
                    WebApi.ClientHandler.CookieContainer.GetCookies(new Uri(ConfigurationManager.ApiSettings.Url))
                        .Cast<Cookie>();

                var token = ExtractToken(loginResponse);
                if (string.IsNullOrWhiteSpace(csrfToken)) throw new InstagramException("Missing csfrtoken");

                ConfigurationManager.AuthSettings.Guid = Guid.NewGuid().ToString();
                ConfigurationManager.AuthSettings.Token = token;
                ConfigurationManager.AuthSettings.UserId = user.LoggedInUser.Id;
                ConfigurationManager.AuthSettings.Cookies = reponseCookie;
                ConfigurationManager.AuthSettings.Save();

                return user;
            }
            return null;
        }

        public async Task<bool> Logout()
        {
            var logout = await WebApi.GetEntityAsync<LogoutResponseMessage>(PostLogout);
            return logout.Success;
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
        public async Task<AutoCompleteUserListResponseMessage> AutoCompleteUser()
        {
            var autoCompleteUserList = await WebApi.GetEntityAsync<AutoCompleteUserListResponseMessage>(GetAutoCompleteUser);
            return autoCompleteUserList;
        }

        public async Task<TimelineFeedResponseMessage> TimeLineFeed()
        {
            var timelineFeed = await WebApi.GetEntityAsync<TimelineFeedResponseMessage>(GetTimelineFeed);
            return timelineFeed;
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