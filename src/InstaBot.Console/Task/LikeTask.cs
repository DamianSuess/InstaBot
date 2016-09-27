using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using InstaBot.Console.Domain;
using InstaBot.Console.Manager;
using InstaBot.Console.Model;
using InstaBot.Console.Model.Event;
using InstaBot.Console.Utils;
using InstaBot.Logging;
using ServiceStack;
using ServiceStack.OrmLite;

namespace InstaBot.Console.Task
{
    public interface ILikeTask : ITask
    {
        void Start();
    }

    public class LikeTask : ILikeTask
    {
        protected ConfigurationManager ConfigurationManager { get; set; }
        protected ILogger Logger { get; set; }
        protected IDbConnection Session { get; set; }
        protected IFeedManager FeedManager { get; set; }
        protected IMediaManager MediaManager { get; set; }
        protected ITagManager TagManager { get; set; }

        public async void Start()
        {
            Logger.Info("Start Like task");
            do
            {
                var medias = new List<Media>();
                var tags = ConfigurationManager.BotSettings.Tags;
                var stopTags = ConfigurationManager.BotSettings.StopTags.Select(x => x.ToUpper()).ToArray();
                Logger.Trace("Using tags:{0}", string.Join(",", tags));
                Logger.Trace("Using stop tags:{0}", string.Join(",", stopTags));

                foreach (var tag in tags)
                {
                    var foundTags = await TagManager.SearchTags(tag);
                    var tagEntities = foundTags.Results.FirstOrDefault(x => x.Name.Equals(tag));
                    if (tagEntities != null)
                    {
                        var tagFeed = await FeedManager.TagFeed(tagEntities.Name);
                        medias.AddRange(
                            tagFeed.Items.Where(
                                x =>
                                    x.LikeCount >= ConfigurationManager.BotSettings.MinLikeToLike &&
                                    x.LikeCount < ConfigurationManager.BotSettings.MaxLikeToLike && !x.HasLiked &&
                                    (x.Caption == null || !x.Caption.Text.ToUpper().ContainsAny(stopTags))));
                    }
                }
                medias = medias.Distinct().ToList();
                medias.Shuffle();
                foreach (var media in medias)
                {
                    if (Session.Select<LikedMedia>(x => x.Id == media.Id).Any()) continue;

                    try
                    {
                        var compareHour = DateTime.Now.AddHours(-1);
                        var compareDay = DateTime.Now.AddDays(-1);
                        while (Session.Select<LikedMedia>(x => x.CreationTime > compareHour).Count >
                               ConfigurationManager.BotSettings.MaxLikePerHour ||
                               Session.Select<LikedMedia>(x => x.CreationTime > compareDay).Count >
                               ConfigurationManager.BotSettings.MaxLikePerDay)
                        {
                            var waitTime = 1;
                            Logger.Info($"Too much like, waiting {waitTime}min");
                            Thread.Sleep(new TimeSpan(0, waitTime, 0));
                        }
                        await MediaManager.Like(media.Id);
                        Logger.Info($"Liking media {media.Id}");
                        Session.Insert(new LikedMedia(media.Id));

                    }
                    catch (InstagramException)
                    {
                        continue;
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 30));
                }
            } while (true);
        }
    }
}