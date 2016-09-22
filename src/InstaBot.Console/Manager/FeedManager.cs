namespace InstaBot.Console.Manager
{
    public interface IFeedManager
    {
        
    }
    public class FeedManager : BaseManager, IFeedManager
    {
        private const string GetTag = "feed/tag/speed/?rank_token={rankToken}&ranked_content={ranked}&";

        public FeedManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }
    }
}