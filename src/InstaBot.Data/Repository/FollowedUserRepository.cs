using System.Data;
using InstaBot.Core.Domain;

namespace InstaBot.Data.Repository
{
    public class FollowedUserRepository : Repository<FollowedUser>
    {
        public FollowedUserRepository(IDbConnection session) : base(session)
        {
        }
    }
}