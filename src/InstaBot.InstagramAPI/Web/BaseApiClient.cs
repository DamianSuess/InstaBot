using System;
using System.Net.Http;

namespace InstaBot.InstagramAPI.Web
{
    public interface IApiClient : IDisposable
    {
        HttpClientHandler ClientHandler { get; }
        HttpClient InnerClient { get;  }
    }

    public abstract class BaseApiClient : IApiClient
    {
        protected BaseApiClient(Uri hostUri, HttpClientHandler clientHandler)
        {
            HostUri = hostUri;
            ClientHandler = clientHandler;
            InnerClient = new HttpClient(clientHandler) { BaseAddress = hostUri };
        }

        public Uri HostUri { get; set; }
        public HttpClientHandler ClientHandler { get; }
        public HttpClient InnerClient { get; protected set; }

        public void Dispose()
        {
            InnerClient.Dispose();
        }
    }
}