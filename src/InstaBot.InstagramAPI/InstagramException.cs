using System;

namespace InstaBot.InstagramAPI
{
    public class InstagramException : Exception
    {
        public InstagramException()
        {
        }

        public InstagramException(string message) : base(message)
        {
        }
    }
}
