using System;

namespace InstaBot.Console.Utils
{
    public static class TimestampExtension
    {
        public static DateTime FromUnixTime(this long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(timestamp);
        }
    }
}