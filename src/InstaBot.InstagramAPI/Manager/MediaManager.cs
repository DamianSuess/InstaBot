using System.Threading.Tasks;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Event;
using InstaBot.InstagramAPI.Settings;
using Newtonsoft.Json.Linq;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IMediaManager : IBaseManager
    {
        Task<LikeResponseMessage> Like(Media media);
        Task<LikeResponseMessage> UnLike(string mediaId);
    }

    public class MediaManager : BaseManager, IMediaManager
    {
        private const string PostLike = "media/{0}/like/";
        private const string PostUnLike = "media/{0}/unlike/";

        public MediaManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }

        public async Task<LikeResponseMessage> Like(Media media)
        {
            MessageHub.PublishAsync(new BeforeLikeEvent(this, media));
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.media_id = media.Id;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse =
                await WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostLike, media.Id), content);
            MessageHub.PublishAsync(new AfterLikeEvent(this, media));
            return likeResponse;
        }

        public async Task<LikeResponseMessage> UnLike(string mediaId)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.media_id = mediaId;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse =
                await WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostUnLike, mediaId), content);
            return likeResponse;
        }

    }
}