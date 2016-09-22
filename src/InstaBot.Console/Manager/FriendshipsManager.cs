namespace InstaBot.Console.Manager
{
    public interface IFriendshipsManager
    {

    }
    public class FriendshipsManager : BaseManager, IFeedManager
    {
        public FriendshipsManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }
    }
}