using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        Task<bool> Logout();
        Task<bool> SyncFeatures();
        Task<AutoCompleteUserListResponseMessage> AutoCompleteUser();
        Task<TimelineFeedResponseMessage> TimeLineFeed();
        Task<UserInfoResponseMessage> UserInfo(string userid);
        Task<FollowResponseMessage> Follow(string userid);
        Task<FollowResponseMessage> UnFollow(string userid);
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
        private const string PostFollow = "friendships/create/{0}/";
        private const string PostUnFollow = "friendships/destroy/{0}/";

        public AccountManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }

        public async Task<LoginResponseMessage> Login(bool force = false)
        {
            if (!force && !string.IsNullOrWhiteSpace(_authSettings.UserId) &&
                !string.IsNullOrWhiteSpace(_authSettings.Token))
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
                new LoginMessage(csrfToken, _authSettings.Login,
                    _authSettings.Password);

            var content = SignedContent(message.ToString());

            var loginResponse = await WebApi.PostLoginAsync(PostLogin, content);
            if (!loginResponse.IsSuccessStatusCode) throw new InstagramException(loginResponse.ReasonPhrase);

            var user =
                JsonConvert.DeserializeObject<LoginResponseMessage>(loginResponse.Content.ReadAsStringAsync().Result);
            if (user.Status.Equals("ok"))
            {
                var reponseCookie =
                    WebApi.ClientHandler.CookieContainer.GetCookies(new Uri(_apiSettings.Url))
                        .Cast<Cookie>();

                var token = ExtractToken(loginResponse);
                if (string.IsNullOrWhiteSpace(csrfToken)) throw new InstagramException("Missing csfrtoken");

                _authSettings.Guid = Guid.NewGuid().ToString();
                _authSettings.Token = token;
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
            var logout = await WebApi.GetEntityAsync<LogoutResponseMessage>(PostLogout);
            return logout.Success;
        }


        public async Task<bool> SyncFeatures()
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.id = _authSettings.UserId;
            syncMessage.experiments = _apiSettings.Experiments;

            var content = SignedContent(syncMessage.ToString());

            var syncEntity = await WebApi.PostEntityAsync<SyncResponseMessage>(GetSync, content);
            return true;
        }
        public async Task<AutoCompleteUserListResponseMessage> AutoCompleteUser()
        {
            var autoCompleteUserList = await WebApi.GetEntityAsync<AutoCompleteUserListResponseMessage>(GetAutoCompleteUser);
            return autoCompleteUserList;
        }

        public async Task<UserInfoResponseMessage> UserInfo(string userid)
        {
            var userInfo = await WebApi.GetEntityAsync<UserInfoResponseMessage>(string.Format(GetUserinfo, userid));
            return userInfo;
        }

        public async Task<TimelineFeedResponseMessage> TimeLineFeed()
        {
            var timelineFeed = await WebApi.GetEntityAsync<TimelineFeedResponseMessage>(GetTimelineFeed);
            return timelineFeed;
        }
        
        public async Task<FollowResponseMessage> Follow(string userId)
        {
            MessageHub.PublishAsync(new BeforeFollowEvent(this, userId));
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.user_id = userId;

            var content = SignedContent(syncMessage.ToString());

            var followReponse = await WebApi.PostEntityAsync<FollowResponseMessage>(string.Format(PostFollow, userId), content);

            MessageHub.PublishAsync(new AfterFollowEvent(this, userId));
            return followReponse;
        }

        public async Task<FollowResponseMessage> UnFollow(string userId)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.user_id = userId;

            var content = SignedContent(syncMessage.ToString());

            var followReponse = await WebApi.PostEntityAsync<FollowResponseMessage>(string.Format(PostUnFollow, userId), content);
            return followReponse;
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