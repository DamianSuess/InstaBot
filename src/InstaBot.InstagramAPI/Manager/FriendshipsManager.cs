using System.Threading;
using System.Threading.Tasks;
using InstaBot.Core;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Event;
using InstaBot.InstagramAPI.Settings;
using Newtonsoft.Json.Linq;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IFriendshipsManager : IBaseManager
    {
        Task<FollowResponseMessage> Follow(string userid);
        Task<FollowResponseMessage> UnFollow(string userid);
        Task<FollowResponseMessage> Follow(string userid, CancellationToken token);
        Task<FollowResponseMessage> UnFollow(string userid, CancellationToken token);
    }
    public class FriendshipsManager : BaseManager, IFriendshipsManager
    {
        private const string PostFollow = "friendships/create/{0}/";
        private const string PostUnFollow = "friendships/destroy/{0}/";

        public FriendshipsManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }


        public async Task<FollowResponseMessage> Follow(string userid)
        {
            return await Follow(userid, CancellationToken.None);
        }

        public async Task<FollowResponseMessage> UnFollow(string userid)
        {
            return await UnFollow(userid, CancellationToken.None);
        }

        public async Task<FollowResponseMessage> Follow(string userId, CancellationToken token)
        {
            MessageHub.PublishAsync(new BeforeFollowEvent(this, userId));
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.user_id = userId;

            var content = SignedContent(syncMessage.ToString());

            var followReponse = await Retry.Do(WebApi.PostEntityAsync<FollowResponseMessage>(string.Format(PostFollow, userId), content, token), token);

            MessageHub.PublishAsync(new AfterFollowEvent(this, userId));
            return followReponse;
        }

        public async Task<FollowResponseMessage> UnFollow(string userId, CancellationToken token)
        {
            dynamic syncMessage = new JObject();
            syncMessage._uuid = _authSettings.Guid;
            syncMessage._uid = _authSettings.UserId;
            syncMessage._csrftoken = _authSettings.Token;
            syncMessage.user_id = userId;

            var content = SignedContent(syncMessage.ToString());

            var followReponse = await Retry.Do(WebApi.PostEntityAsync<FollowResponseMessage>(string.Format(PostUnFollow, userId), content, token), token);
            return followReponse;
        }
    }
}