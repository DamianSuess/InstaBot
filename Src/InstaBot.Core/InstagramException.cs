using System;

namespace InstaBot.Core
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