using System;
using System.Collections.Generic;
using System.Net.Http;
using InstaBot.Console.Utils;
using InstaBot.Console.Web;

namespace InstaBot.Console.Manager
{
    public interface IBaseManager
    {
    }

    public abstract class BaseManager : IBaseManager
    {
        public IInstagramApiClient WebApi { get; set; }

        protected ICollection<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>(new[]
        {
            new KeyValuePair<string, string>("Accept", "*/*"),
            new KeyValuePair<string, string>("Accept-Language", "en-US"),
            new KeyValuePair<string, string>("'Content-type", "application/x-www-form-urlencoded; charset=UTF-8"),
            new KeyValuePair<string, string>("Connection", "Close"),
            new KeyValuePair<string, string>("Cookie2", " $Version=1")
        });

        protected ConfigurationManager ConfigurationManager;

        public BaseManager(ConfigurationManager configurationManager)
        {
            ConfigurationManager = configurationManager;
            WebApi = new InstagramApiClient(new Uri(ConfigurationManager.ApiSettings.Url), new HttpClientHandler());
            //User agent
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