using InstaBot.Console.Manager;

namespace InstaBot.Console.Task
{
    public interface ILogin : ITask
    {
        void DoLogin();
    }

    public class LoginTask : ILogin
    {
        protected ConfigurationManager ConfigurationManager { get; set; }
        protected IAccountManager IAccountManager { get; set; }
        protected IFeedManager FeedManager { get; set; }


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