using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Console.Manager
{
    public class TagManager : BaseManager
    {
        //Ranked token : userid_guid
        private const string GetTag = "/api/v1/feed/tag/speed/?rank_token={rankToken}&ranked_content={ranked}&";

        public TagManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }
    }
}
