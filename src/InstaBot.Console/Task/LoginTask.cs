using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using InstaBot.Console.Manager;
using InstaBot.Console.Model;
using InstaBot.Console.Utils;

namespace InstaBot.Console.Task
{
    public interface ILogin : ITask
    {
        void DoLogin();
    }

    public class LoginTask : ILogin
    {
        protected ConfigurationManager ConfigurationManager;
        protected IUserManager UserManager;
        protected IFeedManager FeedManager;

        public LoginTask(ConfigurationManager configurationManager, IUserManager userManager, IFeedManager feedManager)
        {
            ConfigurationManager = configurationManager;
            UserManager = userManager;
            FeedManager = feedManager;
        }

        public async void DoLogin()
        {
            await UserManager.Login();
            await UserManager.SyncFeatures();
            //await UserManager.AutoCompleteUser();
            await UserManager.TimeLineFeed();
            await FeedManager.Explore();
        }
    }
}