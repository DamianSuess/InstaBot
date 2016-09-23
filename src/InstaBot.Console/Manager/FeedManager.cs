using System.Threading.Tasks;
using InstaBot.Console.Model;

namespace InstaBot.Console.Manager
{
    public interface IFeedManager
    {
        Task<ExploreResponseMessage> Explore();

    }
    public class FeedManager : BaseManager, IFeedManager
    {
        private const string GetExplore = "discover/explore/";
        private const string GetTag = "feed/tag/speed/?rank_token={rankToken}&ranked_content={ranked}&";

        public FeedManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }


        public async Task<ExploreResponseMessage> Explore()
        {
            var explore = await WebApi.GetEntityAsync<ExploreResponseMessage>(GetExplore);
            return explore;
        }
    }
}