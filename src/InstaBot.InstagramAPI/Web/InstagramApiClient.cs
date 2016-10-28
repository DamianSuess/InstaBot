using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InstaBot.InstagramAPI.Domain;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Web
{
    public interface IInstagramApiClient : IApiClient
    {

        Task<T> GetEntityAsync<T>(string url) where T : BaseResponseMessage;
        Task<HttpResponseMessage> PostLoginAsync(string url, HttpContent data);
        Task<T> PostEntityAsync<T>(string url, HttpContent data) where T : BaseResponseMessage;
    }

    public class InstagramApiClient : BaseApiClient, IInstagramApiClient
    {
        public InstagramApiClient(Uri hostUri, HttpClientHandler clientHandler) : base(hostUri, clientHandler)
        {
        }
        
        public ICollection<KeyValuePair<string, string>> Headers { get; set; }

        public async Task<T> GetEntityAsync<T>(string url) where T : BaseResponseMessage
        {
            var response = await SendRequest(url, HttpMethod.Get);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }
        public async Task<HttpResponseMessage> PostLoginAsync(string url, HttpContent data)
        {
            return await SendRequest(url, HttpMethod.Post, data, true);
        }

        public async Task<T> PostEntityAsync<T>(string url, HttpContent data) where T : BaseResponseMessage
        {
            var response = await SendRequest(url, HttpMethod.Post, data);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }

        private async Task<HttpResponseMessage> SendRequest(string url, HttpMethod method, HttpContent data, bool login)
        {
            if (method == null) method = HttpMethod.Get;
            var request = new HttpRequestMessage(method, url);
            if (data != null && method != HttpMethod.Get) request.Content = data;
            AddHeaders(request);
            return await InnerClient.SendAsync(request);
        }
        
        private async Task<HttpResponseMessage> SendRequest(string url, HttpMethod method = null, HttpContent data = null) 
        {
            if (method == null) method = HttpMethod.Get;
            var request = new HttpRequestMessage(method, url);
            if (data != null && method != HttpMethod.Get) request.Content = data;
            AddHeaders(request);
            AddAuth(request);
            return await InnerClient.SendAsync(request);
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Clear();
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
        private void AddAuth(HttpRequestMessage request)
        {

        }

    }
}