using System.Data;
using System.Threading.Tasks;
using InstaBot.Console.Model;

namespace InstaBot.Console.Manager
{
    public interface ITagManager
    {
        Task<TagResponseMessage> SearchTags(string tag);
    }

    public class TagManager : BaseManager, ITagManager
    {
        private const string GetSearchTag = "tags/search/?is_typeahead=true&q={0}&rank_token={1}";

        public TagManager(ConfigurationManager configurationManager, IDbConnection session) : base(configurationManager)
        {
        }

        public async Task<TagResponseMessage> SearchTags(string tag)
        {
            var tags = await WebApi.GetEntityAsync<TagResponseMessage>(string.Format(GetSearchTag, tag, RankToken));
            return tags;
        }
    }
}