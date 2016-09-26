using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using InstaBot.Console.Domain;
using InstaBot.Console.Manager;
using InstaBot.Console.Model;
using InstaBot.Console.Utils;
using InstaBot.Logging;
using ServiceStack;
using ServiceStack.OrmLite;

namespace InstaBot.Console.Task
{
    public interface IFollowingTask : ITask
    {
        void Start();
    }
    public class FollowingTask : IFollowingTask
    {
        protected ConfigurationManager ConfigurationManager;
        protected ILogger Logger;
        protected IDbConnection Session;
        protected IFeedManager FeedManager;
        protected ITagManager TagManager;
        protected IAccountManager AccountManager;

        public FollowingTask(ConfigurationManager configurationManager, ILogger logger, IDbConnection session, IAccountManager accountManager, ITagManager tagManager, IFeedManager feedManager)
        {
            ConfigurationManager = configurationManager;
            Logger = logger;
            Session = session;
            TagManager = tagManager;
            FeedManager = feedManager;
            AccountManager = accountManager;
        }

        public async void Start()
        {
            var stopTags = ConfigurationManager.BotSettings.StopTags.Select(x => x.ToUpper()).ToArray();

            Logger.Info("Start Following task");
            do
            {
                await Follow(stopTags);
                await UnFollow();
            } while (true);
        }

        private  async System.Threading.Tasks.Task UnFollow()
        {
            var compareDate = DateTime.Now.Add(new TimeSpan(-1, 0, 0));
            var unfollowList = Session.Select<FollowedUser>(x => x.FollowTime < compareDate && !x.UnFollowTime.HasValue);
            if (unfollowList.Any())
            {
                foreach (var followedUser in unfollowList)
                {
                    Logger.Info($"UnFollow User {followedUser.Id}, following time was { DateTime.Now.Subtract(followedUser.FollowTime).ToString("g")}");
                    await AccountManager.UnFollow(followedUser.Id);
                    followedUser.UnFollowTime = DateTime.Now;
                    Session.Update(followedUser);
                    Thread.Sleep(new TimeSpan(0, 0, 20));
                }
            }
        }

        private async System.Threading.Tasks.Task Follow(string[] stopTags)
        {
            var exploreReponse = await FeedManager.Explore();
            var medias = exploreReponse.Items.Where(
                x =>
                    x.Media.LikeCount >= ConfigurationManager.BotSettings.MinLikeToLike &&
                    x.Media.LikeCount < ConfigurationManager.BotSettings.MaxLikeToLike && !x.Media.HasLiked &&
                    (x.Media.Caption == null || !x.Media.Caption.Text.ToUpper().ContainsAny(stopTags)));
            foreach (var media in medias)
            {
                var compareDay = DateTime.Now.AddDays(-1);
                while (Session.Select<FollowedUser>(x => x.FollowTime > compareDay).Count >
                       ConfigurationManager.BotSettings.MaxFollowPerDay)
                {
                    var waitTime = 5;
                    Logger.Info($"Too much follow, waiting {waitTime}min");
                    Thread.Sleep(new TimeSpan(0, waitTime, 0));
                }

                var user = await AccountManager.UserInfo(media.Media.User.Id);
                Logger.Info($"Get information for user {user.User.Id}");
                if (Session.Select<FollowedUser>(x => x.Id == user.User.Id).Any()) continue;

                var followingRatio = Convert.ToDouble(Decimal.Divide(user.User.FollowingCount, user.User.FollowerCount));

                if (followingRatio > ConfigurationManager.BotSettings.FollowingRatio)
                {
                    Logger.Info($"Follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    Session.Insert(new FollowedUser(user.User.Id));
                    await AccountManager.Follow(user.User.Id);
                    Thread.Sleep(new TimeSpan(0, 5, 0));
                }
                else
                {
                    Logger.Info($"Skipped follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    Thread.Sleep(new TimeSpan(0, 0, 20));
                }
            }
        }
    }

}