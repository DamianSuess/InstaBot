using System.Threading;
using System.Threading.Tasks;
using InstaBot.Core;
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
        Task<LikeResponseMessage> Like(Media media, CancellationToken token);
        Task<LikeResponseMessage> UnLike(string mediaId, CancellationToken token);
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
            return await Like(media, CancellationToken.None);
        }

        public async Task<LikeResponseMessage> UnLike(string mediaId)
        {
            return await UnLike(mediaId, CancellationToken.None);
        }

        public async Task<LikeResponseMessage> Like(Media media, CancellationToken token)
        {
            MessageHub.PublishAsync(new BeforeLikeEvent(this, media));
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.media_id = media.Id;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse = await Retry.Do(WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostLike, media.Id), content, token), token);

            MessageHub.PublishAsync(new AfterLikeEvent(this, media));
            return likeResponse;
        }

        public async Task<LikeResponseMessage> UnLike(string mediaId, CancellationToken token)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.media_id = mediaId;

            var content = SignedContent(syncMessage.ToString());

            var likeResponse = await Retry.Do(WebApi.PostEntityAsync<LikeResponseMessage>(string.Format(PostUnLike, mediaId), content, token), token);

            return likeResponse;
        }

    }
}