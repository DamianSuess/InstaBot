using System;
using System.Net.Http;
using System.Threading.Tasks;
using InstaBot.InstagramAPI.Domain;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Web
{
    public interface IInstagramApiClient : IApiClient
    {
        Task<T> GetEntityAsync<T>(string url) where T : BaseResponseMessage;
        Task<T> PostEntityAsync<T>(string url, HttpContent data) where T : BaseResponseMessage;
    }
    public class InstagramApiClient : BaseApiClient, IInstagramApiClient
    {
        public InstagramApiClient(Uri hostUri, HttpClientHandler clientHandler) : base(hostUri, clientHandler)
        {
        }

        public async Task<T> GetEntityAsync<T>(string url) where T : BaseResponseMessage
        {
            var response = await InnerClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }

        public async Task<T> PostEntityAsync<T>(string url, HttpContent data) where T : BaseResponseMessage
        {
            var response = await InnerClient.PostAsync(url, data);
            if (!response.IsSuccessStatusCode) throw new InstagramException($"Bad response status code: {response.StatusCode}");
            var entity = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (!entity.Status.Equals("ok")) throw new InstagramException(entity.Message);
            return entity;
        }
    }
}