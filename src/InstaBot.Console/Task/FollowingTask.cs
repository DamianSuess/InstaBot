using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using InstaBot.Core.Domain;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Event;
using InstaBot.InstagramAPI.Manager;
using InstaBot.Logging;
using ServiceStack;
using ServiceStack.OrmLite;
using TinyMessenger;
using System.Threading.Tasks;

namespace InstaBot.Console.Task
{
    public interface IFollowingTask : ITask
    {
        void Start();
    }

    public class FollowingTask : IFollowingTask
    {
        public ConfigurationManager ConfigurationManager { get; set; }
        public ITinyMessengerHub MessageHub { get; set; }
        public ILogger Logger { get; set; }
        public IDbConnection Session { get; set; }
        public IFeedManager FeedManager { get; set; }
        public ITagManager TagManager { get; set; }
        public IAccountManager AccountManager { get; set; }

        private Queue<Media> _usersQueue = new Queue<Media>();

        public async void Start()
        {
            Logger.Info("Start Following task");
            MessageHub.Subscribe<AfterLikeEvent>(LikeMessageReceived);

            Follow();
            UnFollow();
        }

        private void LikeMessageReceived(AfterLikeEvent afterLikeEvent)
        {
            _usersQueue.Enqueue(afterLikeEvent.Entity);
        }

        private async void UnFollow()
        {
            do
            {
                var compareDate = DateTime.Now.Add(new TimeSpan(-3, 0, 0)); //TODO configure time
                var unfollowList = Session.Select<FollowedUser>(x => x.FollowTime < compareDate && !x.UnFollowTime.HasValue);
                if (unfollowList.Any())
                {
                    foreach (var followedUser in unfollowList)
                    {
                        Logger.Info(
                            $"UnFollow User {followedUser.Id}, following time was {DateTime.Now.Subtract(followedUser.FollowTime).ToString("g")}");
                        await AccountManager.UnFollow(followedUser.Id);
                        followedUser.UnFollowTime = DateTime.Now;
                        Session.Update(followedUser);
                        Thread.Sleep(new TimeSpan(0, 0, 20));
                    }
                }
                //Wait for next check
                Thread.Sleep(new TimeSpan(0, 10, 0));
            } while (true);
        }

        private async void Follow()
        {
            Queue<Media> exploreQueue = new Queue<Media>();
            await EnqueueMedia(exploreQueue);

            do
            {
                var compareDay = DateTime.Now.AddDays(-1);
                while (Session.Select<FollowedUser>(x => x.FollowTime > compareDay).Count >
                       ConfigurationManager.BotSettings.MaxFollowPerDay)
                {
                    var waitTime = 5;
                    Logger.Info($"Too much follow, waiting {waitTime}min");
                    Thread.Sleep(new TimeSpan(0, waitTime, 0));
                }

                Media currentMedia = null;
                if (_usersQueue.Any())
                {
                    currentMedia = _usersQueue.Dequeue();
                }
                else
                {
                    if (!exploreQueue.Any())
                        await EnqueueMedia(exploreQueue);
                    if (!exploreQueue.Any())
                        continue;
                    currentMedia = exploreQueue.Dequeue();
                }
                
                if (Session.Select<FollowedUser>(x => x.Id == currentMedia.User.Id).Any()) continue;
                Logger.Info($"Get information for user {currentMedia.User.Id}");
                var user = await AccountManager.UserInfo(currentMedia.User.Id);

                double followingRatio;
                if (user.User.FollowerCount == 0) followingRatio = 1;
                else
                    followingRatio = Convert.ToDouble(decimal.Divide(user.User.FollowingCount, user.User.FollowerCount));

                if (followingRatio > ConfigurationManager.BotSettings.FollowingRatio)
                {
                    Logger.Info($"Follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    Session.Insert(new FollowedUser(user.User.Id));
                    await AccountManager.Follow(user.User.Id);
                    Thread.Sleep(new TimeSpan(0, 1, 0));
                }
                else
                {
                    Logger.Info(
                        $"Skipped follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    Thread.Sleep(new TimeSpan(0, 0, 20));
                }
            } while (true);

        }

        private async Task<bool> EnqueueMedia(Queue<Media> medias)
        {
            string[] stopTags = ConfigurationManager.BotSettings.StopTags;
            var exploreReponse = await FeedManager.Explore();
            foreach (var media in exploreReponse.Medias.Where(
                x =>
                    x.LikeCount >= ConfigurationManager.BotSettings.MinLikeToLike &&
                    x.LikeCount < ConfigurationManager.BotSettings.MaxLikeToLike && !x.HasLiked &&
                    (x.Caption == null || !x.Caption.Text.ToUpper().ContainsAny(stopTags))))
            {
                medias.Enqueue(media);
            }

            return true;
        }
    }
}