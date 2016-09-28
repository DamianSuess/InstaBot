using InstaBot.InstagramAPI.Manager;

namespace InstaBot.Console.Task
{
    public interface ILogin : ITask
    {
        void DoLogin();
    }

    public class LoginTask : ILogin
    {
        public IAccountManager IAccountManager { get; set; }
        public IFeedManager FeedManager { get; set; }


        public async void DoLogin()
        {
            await IAccountManager.Login();
            await IAccountManager.SyncFeatures();
            //await IAccountManager.AutoCompleteUser();
            await IAccountManager.TimeLineFeed();
            await FeedManager.Explore();
        }
    }
}