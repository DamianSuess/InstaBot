using System.Threading.Tasks;
using InstaBot.InstagramAPI.Manager;

namespace InstaBot.Console.Tasks
{
    public interface ILogin : ITask
    {
        Task DoLogin();
    }

    public class LoginTask : ILogin
    {
        public IAccountManager IAccountManager { get; set; }
        public IFeedManager FeedManager { get; set; }


        public async Task DoLogin()
        {
            await IAccountManager.Login();
            await IAccountManager.SyncFeatures();
            //await IAccountManager.AutoCompleteUser();
            await IAccountManager.TimeLineFeed();
            await FeedManager.Explore();
        }
    }
}