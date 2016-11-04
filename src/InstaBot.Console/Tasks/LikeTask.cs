using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstaBot.Console.Utils;
using InstaBot.Core;
using InstaBot.Core.Domain;
using InstaBot.Data.Repository;
using InstaBot.InstagramAPI;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Manager;
using InstaBot.Logging;
using ServiceStack;

namespace InstaBot.Console.Tasks
{
    public interface ILikeTask : ITask
    {
        Task Start();
    }

    public class LikeTask : ILikeTask
    {
        public ConfigurationManager ConfigurationManager { get; set; }
        public ILogger Logger { get; set; }
        public IRepository<LikedMedia> LikedMediaRepository { get; set; }
        public IFeedManager FeedManager { get; set; }
        public IMediaManager MediaManager { get; set; }
        public ITagManager TagManager { get; set; }

        public async Task Start()
        {
            Logger.Info("Start Like task");
            var tags = ConfigurationManager.BotSettings.Tags;
            var stopTags = ConfigurationManager.BotSettings.StopTags.Select(x => x.ToUpper()).ToArray();
            Logger.Info("Using tags:{0}", string.Join(",", tags));
            Logger.Info("Using stop tags:{0}", string.Join(",", stopTags));
            do
            {
                var medias = new List<Media>();
                foreach (var tag in tags)
                {
                    try
                    {
                        var foundTags = await TagManager.SearchTags(tag);
                        var tagEntity = foundTags.Results.FirstOrDefault(x => x.Name.Equals(tag));
                        if (tagEntity == null) continue;

                        Logger.Trace($"Retrieving tag {tagEntity.Name}");

                        var tagFeed = await FeedManager.TagFeed(tagEntity.Name);
                        var resultsTag = tagFeed.Items.Where(x =>
                        x != null &&
                            x.LikeCount >= ConfigurationManager.BotSettings.MinLikeToLike &&
                            x.LikeCount < ConfigurationManager.BotSettings.MaxLikeToLike && !x.HasLiked &&
                            (x.Caption == null || !x.Caption.Text.ToUpper().ContainsAny(stopTags)) &&
                            (x.Comments == null || !x.Comments.Any(c => c.Text.ToUpper().ContainsAny(stopTags)))).ToList();
                        Logger.Trace($"Retrieved {resultsTag.Count} medias for tag {tagEntity.Name}");
                        medias.AddRange(resultsTag);

                    }
                    catch (InstagramException ex)
                    {
                        Logger.Error($"Error on fetching tag {tag}", ex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Critical($"Critical error on fetching tag {tag}", ex);
                    }

                }
                medias = medias.Distinct().ToList();
                medias.Shuffle();
                foreach (var media in medias)
                {
                    if (LikedMediaRepository.GetById(media.Id) != null) continue;

                    try
                    {
                        while (NeedWaitToNextLike())
                        {
                            var waitTime = 5;
                            Logger.Info($"Too much like, waiting {waitTime}min");
                            await Task.Delay(new TimeSpan(0, waitTime, 0));
                        }

                        await MediaManager.Like(media);
                        Logger.Info($"Liking media {media.Id}");
                        LikedMediaRepository.Save(new LikedMedia(media.Id));
                        await Task.Delay(new TimeSpan(0, 0, 30));
                    }
                    catch (InstagramException ex)
                    {
                        Logger.Error($"Unable to like {media.Id}, {ex}", ex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Fatal erreur to like {media.Id}, {ex}", ex);
                    }
                }
            } while (true);
        }

        private bool NeedWaitToNextLike()
        {
            var compareHour = DateTime.Now.AddHours(-1);
            var compareDay = DateTime.Now.AddDays(-1);
            var hourLike = LikedMediaRepository.Query<LikedMedia>(x => x.CreationTime > compareHour).Count();
            var dailyLike = LikedMediaRepository.Query<LikedMedia>(x => x.CreationTime > compareDay).Count();
            Logger.Trace($"{hourLike} like since {compareHour}");
            Logger.Trace($"{dailyLike} like since {compareDay}");
            return hourLike > ConfigurationManager.BotSettings.MaxLikePerHour || dailyLike > ConfigurationManager.BotSettings.MaxLikePerDay;
        }
    }
}