using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaBot.Console.Model;
using Newtonsoft.Json.Linq;

namespace InstaBot.Console.Manager
{
    public class MediaManager : BaseManager
    {
        private const string PostLike = "media/{mediaId}/like/";

        public MediaManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }

        public async Task<LikeResponseMessage> Like(int mediaId)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = ConfigurationManager.AuthSettings.Guid;
            syncMessage._uid = ConfigurationManager.AuthSettings.UserId;
            syncMessage._csrftoken = ConfigurationManager.AuthSettings.Token;
            syncMessage.media_id = mediaId;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse = await WebApi.PostEntityAsync<LikeResponseMessage>(PostLike, content);
            return likeResponse;
        }

    }
}
