namespace InstaBot.Console.Manager
{
    public interface IFriendshipsManager
    {

    }
    public class FriendshipsManager : BaseManager, IFriendshipsManager
    {
        public FriendshipsManager(ConfigurationManager configurationManager) : base(configurationManager)
        {
        }
    }
}