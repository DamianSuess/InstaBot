using System;
using System.Net.Http;

namespace InstaBot.Console.Web
{
    public interface IApiClient : IDisposable
    {
        HttpClient InnerClient { get; }
    }

    public abstract class BaseApiClient : IApiClient
    {
        protected BaseApiClient(Uri hostUri, HttpClientHandler clientHandler)
        {
            InnerClient = new HttpClient(clientHandler) {BaseAddress = hostUri};
        }

        public HttpClient InnerClient { get; }

        public void Dispose()
        {
            InnerClient.Dispose();
        }
    }
}