using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Console.Manager
{
    public class MediaManager : BaseManager
    {
        private const string Like = "/api/v1/media/{mediaId}/like/";

        public MediaManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }
    }
}
