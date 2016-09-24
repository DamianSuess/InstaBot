using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InstaBot.Console.Manager;
using InstaBot.Console.Model;
using InstaBot.Console.Utils;

namespace InstaBot.Console.Task
{
    public interface ILikeTask : ITask
    {
        void Start();
    }
    public class LikeTask : ILikeTask
    {
        protected ConfigurationManager ConfigurationManager;
        protected ITagManager TagManager;
        protected IFeedManager FeedManager;
        protected IMediaManager MediaManager;
        public LikeTask(ConfigurationManager configurationManager, ITagManager tagManager, IFeedManager feedManager, IMediaManager mediaManager)
        {
            ConfigurationManager = configurationManager;
            TagManager = tagManager;
            FeedManager = feedManager;
            MediaManager = mediaManager;
        }

        public async void Start()
        {
            List<Media> medias = new List<Media>();
            var tags = new[] { "photography", "landscape" };
            foreach (var tag in tags)
            {
                var foundTags = await TagManager.SearchTags(tag);
                var tagEntities = foundTags.Results.FirstOrDefault(x => x.Name.Equals(tag));
                if (tagEntities != null)
                {
                    var tagFeed = await FeedManager.TagFeed(tagEntities.Name);
                    medias.AddRange(tagFeed.Items.Where(x => x.LikeCount > 0 && x.LikeCount < 50 && (x.Caption == null || !x.Caption.Text.Contains("#porn"))));
                }
            }
            medias = medias.Distinct().ToList();
            medias.Shuffle();
            foreach (var media in medias)
            {
                await MediaManager.Like(media.Id);
                Thread.Sleep(new TimeSpan(0, 0, 0, 40));
            }
        }
    }


}