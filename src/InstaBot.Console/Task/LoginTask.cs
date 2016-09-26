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
        protected IAccountManager IAccountManager;
        protected IFeedManager FeedManager;

        public LoginTask(ConfigurationManager configurationManager, IAccountManager accountManager, IFeedManager feedManager)
        {
            ConfigurationManager = configurationManager;
            IAccountManager = accountManager;
            FeedManager = feedManager;
        }

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