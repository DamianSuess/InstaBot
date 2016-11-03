using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, string msg, Exception exception, params object[] args);
    }
}
