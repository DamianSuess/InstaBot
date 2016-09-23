using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Console
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
