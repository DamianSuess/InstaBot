using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using InstaBot.Console.Utils;
using InstaBot.Console.Web;
using TinyMessenger;

namespace InstaBot.Console.Manager
{
    public interface IBaseManager
    {
    }

    public abstract class BaseManager : IBaseManager
    {
        public IInstagramApiClient WebApi { get; private set; }
        public ITinyMessengerHub MessageHub { get; set; }

        protected ConfigurationManager ConfigurationManager { get; set; }

        protected ICollection<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>(new[]
        {
            new KeyValuePair<string, string>("Accept", "*/*"),
            new KeyValuePair<string, string>("Accept-Language", "en-US"),
            new KeyValuePair<string, string>("'Content-type", "application/x-www-form-urlencoded; charset=UTF-8"),
            new KeyValuePair<string, string>("Connection", "Close"),
            new KeyValuePair<string, string>("Cookie2", " $Version=1")
        });


        public BaseManager()
        {
            InitializeClient();
            InitializeHeader();
        }


        private void InitializeClient()
        {
            var uri = new Uri(ConfigurationManager.ApiSettings.Url);
            CookieContainer cookies = new CookieContainer();
            if (ConfigurationManager.AuthSettings.Cookies != null && ConfigurationManager.AuthSettings.Cookies.Any())
            {
                foreach (var cookie in ConfigurationManager.AuthSettings.Cookies)
                {
                    cookies.Add(uri, cookie);
                }
            }
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            WebApi = new InstagramApiClient(uri, handler);
        }

        private void InitializeHeader()
        {
            _headers.Add(new KeyValuePair<string, string>("User-Agent",
                $"Instagram {ConfigurationManager.ApiSettings.Version} Android ({ConfigurationManager.ApiSettings.AndroidVersion}/{ConfigurationManager.ApiSettings.AndroidRelease}; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)"));
            foreach (var header in _headers)
            {
                WebApi.InnerClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        protected string SignBody(string message)
        {
            return $"{Crypto.CreateToken(message, ConfigurationManager.ApiSettings.SignKey)}.{message}";
        }
        
        protected FormUrlEncodedContent SignedContent(string message)
        {
            var version = ConfigurationManager.ApiSettings.SignKeyVersion;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ig_sig_key_version", version.ToString()),
                new KeyValuePair<string, string>("signed_body", SignBody(message))
            });
            return content;
        }

        public string RankToken
        {
            get
            {
                return $"{ConfigurationManager.AuthSettings.UserId}_{ConfigurationManager.AuthSettings.Guid}"; 
                
            } 
        }
    }

}