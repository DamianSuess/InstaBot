using System.Data;
using InstaBot.Core.Domain;

namespace InstaBot.Data.Repository
{
    public class LikedMediaRepository : Repository<LikedMedia>
    {
        public LikedMediaRepository(IDbConnection session) : base(session)
        {
        }
    }
}