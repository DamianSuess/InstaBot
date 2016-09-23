using System;
using System.Linq;
using InstaBot.Console.Manager;

namespace InstaBot.Console.Task
{
    public interface ILikeTask
    {
        void Start();
    }
    public class LikeTask : ILikeTask
    {
        protected ConfigurationManager ConfigurationManager;
        protected ITagManager TagManager;
        public LikeTask(ConfigurationManager configurationManager, ITagManager tagManager)
        {
            ConfigurationManager = configurationManager;
            TagManager = tagManager;
        }

        public async void Start()
        {
            var tag = "snow";
            var tags = await TagManager.SearchTags(tag);
            var tagEntity = tags.Results.FirstOrDefault(x => x.Name.Equals(tag));
            if (tagEntity != null)
            {
                
            }
        }
    }

   
}