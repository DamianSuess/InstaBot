using InstaBot.InstagramAPI.Settings;

namespace InstaBot.InstagramAPI.Manager
{
    public interface IFriendshipsManager : IBaseManager
    {

    }
    public class FriendshipsManager : BaseManager, IFriendshipsManager
    {
        public FriendshipsManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }
    }
}