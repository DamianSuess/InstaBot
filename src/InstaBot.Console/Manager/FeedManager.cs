using System;
using System.Data;
using System.Threading.Tasks;
using InstaBot.Console.Model;

namespace InstaBot.Console.Manager
{
    public interface IFeedManager
    {
        Task<ExploreResponseMessage> Explore();
        Task<TagFeedResponseMessage> TagFeed(string tag, int? maxId = null);

    }
    public class FeedManager : BaseManager, IFeedManager
    {
        private const string GetExplore = "discover/explore/";
        //private const string GetTag = "feed/tag/speed/?rank_token={rankToken}&ranked_content={ranked}&";
        private const string GetTag = "feed/tag/{0}/";
        

        public async Task<ExploreResponseMessage> Explore()
        {
            var explore = await WebApi.GetEntityAsync<ExploreResponseMessage>(GetExplore);
            return explore;
        }


        public async Task<TagFeedResponseMessage> TagFeed(string tag, int? maxId = null)
        {
            var endpoint = string.Format(GetTag, tag);
            if (maxId != null && maxId > 0)
                endpoint = $"{endpoint}?max_id={maxId}";
            if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException(nameof(tag));
            var feed = await WebApi.GetEntityAsync<TagFeedResponseMessage>(endpoint);
            return feed;
        }
    }
}