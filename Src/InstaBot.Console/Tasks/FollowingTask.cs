using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstaBot.Core.Domain;
using InstaBot.Data.Repository;
using InstaBot.InstagramAPI;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Event;
using InstaBot.InstagramAPI.Manager;
using InstaBot.Logging;
using ServiceStack;
using TinyMessenger;

namespace InstaBot.Console.Tasks
{
    public interface IFollowingTask : ITask
    {
        Task Start();
    }

    public class FollowingTask : IFollowingTask
    {
        private readonly Queue<Media> _usersQueue = new Queue<Media>();
        public ConfigurationManager ConfigurationManager { get; set; }
        public ITinyMessengerHub MessageHub { get; set; }
        public ILogger Logger { get; set; }
        public IRepository<FollowedUser> RepositoryFollowedUser { get; set; }
        public IFeedManager FeedManager { get; set; }
        public ITagManager TagManager { get; set; }
        public IAccountManager AccountManager { get; set; }
        public IFriendshipsManager FriendshipsManager { get; set; }

        public async Task Start()
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

        private async Task UnFollow()
        {
            do
            {
                var compareDate = DateTime.Now.Add(new TimeSpan(-6, 0, 0)); //TODO configure time
                var unfollowList =
                    RepositoryFollowedUser.Query<FollowedUser>(
                        x => x.FollowTime < compareDate && !x.UnFollowTime.HasValue);
                if (unfollowList.Any())
                {
                    foreach (var followedUser in unfollowList)
                    {
                        Logger.Info($"UnFollow User {followedUser.Id}, following time was {DateTime.Now.Subtract(followedUser.FollowTime).ToString("g")}");
                        try
                        {
                            await FriendshipsManager.UnFollow(followedUser.Id);
                        }
                        catch (Exception ex)
                        {
                            Logger.Critical($"Critical error on unfollowing user {followedUser.Id}", ex);
                            continue;
                        }
                        followedUser.UnFollowTime = DateTime.Now;
                        RepositoryFollowedUser.Save(followedUser);
                        await Task.Delay(new TimeSpan(0, 0, 20));
                    }
                }
                //Wait for next check
                Logger.Trace($"Next unfollow Check in {10}mins");
                await Task.Delay(new TimeSpan(0, 10, 0));
            } while (true);
        }

        private async Task Follow()
        {
            var exploreQueue = new Queue<Media>();
            await EnqueueMedia(exploreQueue);

            do
            {
                var compareDay = DateTime.Now.AddDays(-1);
                var dailyFollow = RepositoryFollowedUser.Query<FollowedUser>(x => x.FollowTime > compareDay).Count();
                Logger.Trace($"{dailyFollow} follow since {compareDay}");
                while (dailyFollow > ConfigurationManager.BotSettings.MaxFollowPerDay)
                {
                    var waitTime = 5;
                    Logger.Info($"Too much follow, waiting {waitTime}min");
                    await Task.Delay(new TimeSpan(0, waitTime, 0));
                }

                Media currentMedia = null;
                if (_usersQueue.Any())
                {
                    currentMedia = _usersQueue.Dequeue();
                    Logger.Trace($"Dequeue from medias");
                }
                else
                {
                    if (!exploreQueue.Any())
                        await EnqueueMedia(exploreQueue);
                    if (!exploreQueue.Any())
                    {
                        Logger.Trace($"No media in explore queue");
                        continue;
                    }
                    currentMedia = exploreQueue.Dequeue();
                    Logger.Trace($"Dequeue media id {currentMedia.Id} from explore queue");
                }

                if (RepositoryFollowedUser.Query<FollowedUser>(x => x.Id == currentMedia.User.Id).Any()) continue;
                Logger.Info($"Get information for user {currentMedia.User.Id}");
                UserInfoResponseMessage user;
                try
                {
                    user = await AccountManager.UserInfo(currentMedia.User.Id);
                }
                catch (Exception ex)
                {
                    Logger.Critical($"Critical error on fetching user {currentMedia.User.Id} informations", ex);
                    continue;
                }

                double followingRatio;
                if (user.User.FollowerCount == 0) followingRatio = 1;
                else
                    followingRatio = Convert.ToDouble(decimal.Divide(user.User.FollowingCount, user.User.FollowerCount));

                if (followingRatio > ConfigurationManager.BotSettings.FollowingRatio)
                {
                    Logger.Info($"Follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    RepositoryFollowedUser.Save(new FollowedUser(user.User.Id));
                    try
                    {
                        await FriendshipsManager.Follow(user.User.Id);
                    }
                    catch (InstagramException ex)
                    {
                        Logger.Error($"Unable to follow {user.User.Id}, {ex}", ex);
                        await Task.Delay(new TimeSpan(0, 1, 30));
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Fatal erreur to follow {user.User.Id}, {ex}", ex);
                    }
                    await Task.Delay(new TimeSpan(0, 1, 30));
                }
                else
                {
                    Logger.Info(
                        $"Skipped follow User {user.User.Id}, following ratio is {Math.Round(followingRatio, 2)}");
                    await Task.Delay(new TimeSpan(0, 0, 5));
                }
            } while (true);
        }

        private async Task EnqueueMedia(Queue<Media> medias)
        {
            Logger.Trace($"Adding medias to explore queue");
            var stopTags = ConfigurationManager.BotSettings.StopTags;

            ExploreResponseMessage exploreReponse;
            try
            {
                exploreReponse = await FeedManager.Explore();
            }
            catch (Exception ex)
            {
                Logger.Critical($"Critical error on adding medias to explore queue", ex);
                return;
            }
            foreach (var media in exploreReponse.Medias.Where(
                x =>
                    x != null &&
                    x.LikeCount >= ConfigurationManager.BotSettings.MinLikeToLike &&
                    x.LikeCount < ConfigurationManager.BotSettings.MaxLikeToLike && !x.HasLiked &&
                    (x.Caption == null || !x.Caption.Text.ToUpper().ContainsAny(stopTags))))
            {
                medias.Enqueue(media);
            }
        }
    }
}