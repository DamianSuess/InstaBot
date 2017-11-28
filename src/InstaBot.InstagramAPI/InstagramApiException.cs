using System;
using System.Net;
using InstaBot.Core;
using InstaBot.InstagramAPI.Domain;

namespace InstaBot.InstagramAPI
{
    public class InstagramApiException : InstagramException
    {
        public HttpStatusCode StatusCode { get; set; }
        public BaseResponseMessage ReponseMessage { get; set; }

        public InstagramApiException()
        {
        }

        public InstagramApiException(string message) : base(message)
        {
        }
        public InstagramApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public InstagramApiException(HttpStatusCode statusCode, BaseResponseMessage entity, string message) : base(message)
        {
            StatusCode = statusCode;
            ReponseMessage = entity;
        }
    }
}
