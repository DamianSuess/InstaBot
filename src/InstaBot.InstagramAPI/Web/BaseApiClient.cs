using System;
using System.Net.Http;

namespace InstaBot.InstagramAPI.Web
{
    public interface IApiClient : IDisposable
    {
        HttpClientHandler ClientHandler { get; }
        HttpClient InnerClient { get; }
    }

    public abstract class BaseApiClient : IApiClient
    {
        protected BaseApiClient(Uri hostUri, HttpClientHandler clientHandler)
        {
            ClientHandler = clientHandler;
            InnerClient = new HttpClient(clientHandler) { BaseAddress = hostUri };
        }

        public HttpClientHandler ClientHandler { get; }
        public HttpClient InnerClient { get; }

        public void Dispose()
        {
            InnerClient.Dispose();
        }
    }
}