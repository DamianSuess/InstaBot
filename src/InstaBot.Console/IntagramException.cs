using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Console
{
    public class IntagramException : Exception
    {
        public IntagramException()
        {
        }

        public IntagramException(string message) : base(message)
        {
        }
    }
}
