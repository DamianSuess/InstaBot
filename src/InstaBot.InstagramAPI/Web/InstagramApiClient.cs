using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
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
        Task<T> GetEntityAsync<T>(string url, CancellationToken token) where T : BaseResponseMessage;
        Task<HttpResponseMessage> PostLoginAsync(string url, HttpContent data, CancellationToken token);
        Task<T> PostEntityAsync<T>(string url, HttpContent data, CancellationToken token) where T : BaseResponseMessage;
    }

    public class InstagramApiClient : BaseApiClient, IInstagramApiClient
    {
        public InstagramApiClient(Uri hostUri, HttpClientHandler clientHandler) : base(hostUri, clientHandler)
        {
        }
        
        public ICollection<KeyValuePair<string, string>> Headers { get; set; }

        public async Task<T> GetEntityAsync<T>(string url) where T : BaseResponseMessage
        {
            return await GetEntityAsync<T>(url, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> PostLoginAsync(string url, HttpContent data)
        {
            return await PostLoginAsync(url, data, CancellationToken.None);
        }

        public async Task<T> PostEntityAsync<T>(string url, HttpContent data) where T : BaseResponseMessage
        {
            return await PostEntityAsync<T>(url, data, CancellationToken.None);
        }

        public async Task<T> GetEntityAsync<T>(string url, CancellationToken token) where T : BaseResponseMessage
        {
            var response = await SendRequest(url, token, HttpMethod.Get);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }
        public async Task<HttpResponseMessage> PostLoginAsync(string url, HttpContent data, CancellationToken token)
        {
            return await SendRequest(url, token, HttpMethod.Post, data, true);
        }

        public async Task<T> PostEntityAsync<T>(string url, HttpContent data, CancellationToken token) where T : BaseResponseMessage
        {
            var response = await SendRequest(url, token, HttpMethod.Post, data);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }

        private async Task<HttpResponseMessage> SendRequest(string url, CancellationToken token, HttpMethod method = null, HttpContent data = null, bool login = false) 
        {
            if (method == null) method = HttpMethod.Get;
            var request = new HttpRequestMessage(method, url);
            if (data != null && method != HttpMethod.Get) request.Content = data;
            AddHeaders(request);
            if(!login)
                AddAuth(request);
            return await InnerClient.SendAsync(request, token);
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