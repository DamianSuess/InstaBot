using System.Threading.Tasks;
using InstaBot.InstagramAPI.Domain;
using InstaBot.InstagramAPI.Settings;

namespace InstaBot.InstagramAPI.Manager
{
    public interface ITagManager : IBaseManager
    {
        Task<TagResponseMessage> SearchTags(string tag);
    }

    public class TagManager : BaseManager, ITagManager
    {
        private const string GetSearchTag = "tags/search/?is_typeahead=true&q={0}&rank_token={1}";

        public TagManager(IApiSettings apiSettings, IAuthSettings authSettings) : base(apiSettings, authSettings)
        {
        }

        public async Task<TagResponseMessage> SearchTags(string tag)
        {
            var tags = await WebApi.GetEntityAsync<TagResponseMessage>(string.Format(GetSearchTag, tag, RankToken));
            return tags;
        }

    }
}