using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InstaBot.Console.Domain;
using InstaBot.Console.Model;
using Newtonsoft.Json.Linq;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

namespace InstaBot.Console.Manager
{
    public interface IMediaManager
    {
        Task<LikeResponseMessage> Like(string mediaId);
        Task<LikeResponseMessage> UnLike(string mediaId);
    }
    public class MediaManager : BaseManager, IMediaManager
    {
        private const string PostLike = "media/{0}/like/";
        private const string PostUnLike = "media/{0}/unlike/";

        public MediaManager(ConfigurationManager configurationManager, IDbConnection session) : base(configurationManager)
        {
        }

        public async Task<LikeResponseMessage> Like(string mediaId)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = ConfigurationManager.AuthSettings.Guid;
            syncMessage._uid = ConfigurationManager.AuthSettings.UserId;
            syncMessage._csrftoken = ConfigurationManager.AuthSettings.Token;
            syncMessage.media_id = mediaId;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse = await WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostLike, mediaId), content);
            return likeResponse;
        }

        public async Task<LikeResponseMessage> UnLike(string mediaId)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = ConfigurationManager.AuthSettings.Guid;
            syncMessage._uid = ConfigurationManager.AuthSettings.UserId;
            syncMessage._csrftoken = ConfigurationManager.AuthSettings.Token;
            syncMessage.media_id = mediaId;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse = await WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostUnLike, mediaId), content);
            return likeResponse;
        }

    }
}
