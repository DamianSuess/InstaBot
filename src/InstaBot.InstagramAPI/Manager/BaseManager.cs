using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using InstaBot.InstagramAPI.Settings;
using InstaBot.InstagramAPI.Utils;
using InstaBot.InstagramAPI.Web;
using TinyMessenger;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IBaseManager
    {
    }

    public abstract class BaseManager : IBaseManager
    {
        protected ICollection<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>(new[]
        {
            new KeyValuePair<string, string>("Accept", "*/*"),
            new KeyValuePair<string, string>("Accept-Language", "en-US"),
            new KeyValuePair<string, string>("'Content-type", "application/x-www-form-urlencoded; charset=UTF-8"),
            new KeyValuePair<string, string>("Connection", "Close"),
            new KeyValuePair<string, string>("Cookie2", " $Version=1")
        });


        public BaseManager(IApiSettings apiSettings, IAuthSettings authSettings)
        {
            _apiSettings = apiSettings;
            _authSettings = authSettings;
            InitializeClient();
        }

        public static IInstagramApiClient WebApi { get; private set; }
        public ITinyMessengerHub MessageHub { get; set; }
        protected IApiSettings _apiSettings { get; set; }
        protected IAuthSettings _authSettings { get; set; }

        public string RankToken
        {
            get { return $"{_authSettings.UserId}_{_authSettings.Guid}"; }
        }


        private void InitializeClient()
        {
            var uri = new Uri(_apiSettings.Url);
            var handler = new HttpClientHandler();
            _headers.Add(new KeyValuePair<string, string>("User-Agent",
               $"Instagram {_apiSettings.Version} Android ({_apiSettings.AndroidVersion}/{_apiSettings.AndroidRelease}; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)"));
            WebApi = new InstagramApiClient(uri, handler) { Headers = _headers };
        }

        public void UpdateAuth()
        {
            var uri = new Uri(_apiSettings.Url);
            var cookies = new CookieContainer();
            if (_authSettings.Cookies != null && _authSettings.Cookies.Any())
            {
                foreach (var cookie in _authSettings.Cookies)
                {
                    cookies.Add(uri, cookie);
                }
            }
            
            var handler = new HttpClientHandler();
            _headers.Add(new KeyValuePair<string, string>("User-Agent",
               $"Instagram {_apiSettings.Version} Android ({_apiSettings.AndroidVersion}/{_apiSettings.AndroidRelease}; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)"));
            handler.CookieContainer = cookies;
            WebApi = new InstagramApiClient(uri, handler) { Headers = _headers };
        }

        protected string SignBody(string message)
        {
            return $"{Crypto.CreateToken(message, _apiSettings.SignKey)}.{message}";
        }

        protected FormUrlEncodedContent SignedContent(string message)
        {
            var version = _apiSettings.SignKeyVersion;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ig_sig_key_version", version.ToString()),
                new KeyValuePair<string, string>("signed_body", SignBody(message))
            });
            return content;
        }
    }
}