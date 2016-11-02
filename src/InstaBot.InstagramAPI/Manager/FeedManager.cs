using System;
using System.Threading;
using System.Threading.Tasks;
using InstaBot.Core;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Settings;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IFeedManager : IBaseManager
    {
        Task<ExploreResponseMessage> Explore();
        Task<TagFeedResponseMessage> TagFeed(string tag, int? maxId = null);
        Task<ExploreResponseMessage> Explore(CancellationToken token);
        Task<TagFeedResponseMessage> TagFeed(string tag, CancellationToken token, int? maxId = null);
    }

    public class FeedManager : BaseManager, IFeedManager
    {
        private const string GetExplore = "discover/explore/";
        //private const string GetTag = "feed/tag/speed/?rank_token={rankToken}&ranked_content={ranked}&";
        private const string GetTag = "feed/tag/{0}/";

        public FeedManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }

        public async Task<ExploreResponseMessage> Explore()
        {
            return await Explore(CancellationToken.None);
        }

        public async Task<TagFeedResponseMessage> TagFeed(string tag, int? maxId = null)
        {
            return await TagFeed(tag, CancellationToken.None, maxId);
        }

        public async Task<ExploreResponseMessage> Explore(CancellationToken token)
        {
            var explore = await Retry.Do(WebApi.GetEntityAsync<ExploreResponseMessage>(GetExplore, token), token);
            return explore;
        }


        public async Task<TagFeedResponseMessage> TagFeed(string tag, CancellationToken token, int? maxId = null)
        {
            var endpoint = string.Format(GetTag, tag);
            if (maxId != null && maxId > 0)
                endpoint = $"{endpoint}?max_id={maxId}";
            if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException(nameof(tag));
            var feed = await Retry.Do(WebApi.GetEntityAsync<TagFeedResponseMessage>(endpoint, token), token);
            return feed;
        }

    }
}