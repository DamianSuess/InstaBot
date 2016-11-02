using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Event;
using InstaBot.InstagramAPI.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IAccountManager : IBaseManager
    {
        Task<LoginResponseMessage> Login(bool force = false);
        Task<LoginResponseMessage> Login(CancellationToken token, bool force = false);
        Task<bool> Logout();
        Task<bool> Logout(CancellationToken token);
        Task<bool> SyncFeatures();
        Task<bool> SyncFeatures(CancellationToken token);
        Task<AutoCompleteUserListResponseMessage> AutoCompleteUser();
        Task<AutoCompleteUserListResponseMessage> AutoCompleteUser(CancellationToken token);
        Task<TimelineFeedResponseMessage> TimeLineFeed();
        Task<TimelineFeedResponseMessage> TimeLineFeed(CancellationToken token);
        Task<UserInfoResponseMessage> UserInfo(string userid);
        Task<UserInfoResponseMessage> UserInfo(string userid, CancellationToken token);
    }

    public class AccountManager : BaseManager, IAccountManager
    {
        private const string GetInfo = "users/{userId}/info/";
        private const string GetHeader = "si/fetch_headers/?challenge_type=signup&guid={0}";
        private const string PostLogin = "accounts/login/";
        private const string PostLogout = "accounts/logout/";
        private const string GetSync = "qe/sync/";
        private const string GetAutoCompleteUser = "friendships/autocomplete_user_list/?version=2";
        private const string GetTimelineFeed = "feed/timeline/";
        private const string GetUserinfo = "users/{0}/info/";

        public AccountManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }

        public async Task<LoginResponseMessage> Login(bool force = false)
        {
            return await Login(CancellationToken.None, force);
        }
        public async Task<LoginResponseMessage> Login(CancellationToken token, bool force = false)
        {
            if (!force && !string.IsNullOrWhiteSpace(_authSettings.UserId) &&
                !string.IsNullOrWhiteSpace(_authSettings.Token))
            {
                try
                {
                    var timeline = await TimeLineFeed(token);
                    if (!string.IsNullOrWhiteSpace(timeline.Message) && timeline.Message.Equals("login_required"))
                        await Login(token, true);
                }
                catch (InstagramException ex)
                {
                    //TODO logger
                    return await Login(token, true);
                }

                return null;
            }
            var response = await WebApi.InnerClient.GetAsync(string.Format(GetHeader, Guid.NewGuid().ToString("N")), token);

            if (!response.IsSuccessStatusCode)
                throw new InstagramException("Couldn't get challenge, check your connection"); ;
            var csrfToken = ExtractToken(response);
            if (string.IsNullOrWhiteSpace(csrfToken)) throw new InstagramException("Missing csfrtoken");

            var message =
                new LoginMessage(csrfToken, _authSettings.Login,
                    _authSettings.Password);

            var content = SignedContent(message.ToString());

            var loginResponse = await WebApi.PostLoginAsync(PostLogin, content, token);
            if (!loginResponse.IsSuccessStatusCode) throw new InstagramException(loginResponse.ReasonPhrase);

            var user =
                JsonConvert.DeserializeObject<LoginResponseMessage>(loginResponse.Content.ReadAsStringAsync().Result);
            if (user.Status.Equals("ok"))
            {
                var reponseCookie =
                    WebApi.ClientHandler.CookieContainer.GetCookies(new Uri(_apiSettings.Url))
                        .Cast<Cookie>();

                var cookieToken = ExtractToken(loginResponse);
                if (string.IsNullOrWhiteSpace(csrfToken)) throw new InstagramException("Missing csfrtoken");

                _authSettings.Guid = Guid.NewGuid().ToString();
                _authSettings.Token = cookieToken;
                _authSettings.UserId = user.LoggedInUser.Id;
                _authSettings.Cookies = reponseCookie;
                _authSettings.Save();
                UpdateAuth();

                return user;
            }
            return null;
        }

        public async Task<bool> Logout()
        {
            return await Logout(CancellationToken.None); 
        }

        public async Task<bool> Logout(CancellationToken token)
        {
            var logout = await WebApi.GetEntityAsync<LogoutResponseMessage>(PostLogout, token);
            return logout.Success;
        }


        public async Task<bool> SyncFeatures()
        {
            return await SyncFeatures(CancellationToken.None);
        }
        public async Task<bool> SyncFeatures(CancellationToken token)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.id = _authSettings.UserId;
            syncMessage.experiments = _apiSettings.Experiments;

            var content = SignedContent(syncMessage.ToString());

            var syncEntity = await WebApi.PostEntityAsync<SyncResponseMessage>(GetSync, content, token);
            return true;
        }

        public async Task<AutoCompleteUserListResponseMessage> AutoCompleteUser()
        {
            return await AutoCompleteUser(CancellationToken.None);
        }
        public async Task<AutoCompleteUserListResponseMessage> AutoCompleteUser(CancellationToken token)
        {
            var autoCompleteUserList = await WebApi.GetEntityAsync<AutoCompleteUserListResponseMessage>(GetAutoCompleteUser, token);
            return autoCompleteUserList;
        }

        public async Task<UserInfoResponseMessage> UserInfo(string userid)
        {
            return await UserInfo(userid, CancellationToken.None);
        }
        public async Task<UserInfoResponseMessage> UserInfo(string userid, CancellationToken token)
        {
            var userInfo = await WebApi.GetEntityAsync<UserInfoResponseMessage>(string.Format(GetUserinfo, userid), token);
            return userInfo;
        }

        public async Task<TimelineFeedResponseMessage> TimeLineFeed()
        {
            return await TimeLineFeed(CancellationToken.None);
        }
        public async Task<TimelineFeedResponseMessage> TimeLineFeed(CancellationToken token)
        {
            var timelineFeed = await WebApi.GetEntityAsync<TimelineFeedResponseMessage>(GetTimelineFeed, token);
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
                    cookieContainer.SetCookies(new Uri(_apiSettings.Url), c);
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