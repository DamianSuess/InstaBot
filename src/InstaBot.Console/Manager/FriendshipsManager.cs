using System.Data;

namespace InstaBot.Console.Manager
{
    public interface IFriendshipsManager
    {

    }
    public class FriendshipsManager : BaseManager, IFriendshipsManager
    {
        public FriendshipsManager(ConfigurationManager configurationManager, IDbConnection session) : base(configurationManager)
        {
        }
    }
}