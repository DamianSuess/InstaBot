using System;
using ServiceStack.DataAnnotations;

namespace InstaBot.Core.Domain
{
    public class FollowedUser
    {
        public FollowedUser(string id)
        {
            Id = id;
            FollowTime = DateTime.Now;
            UnFollowTime = null;
        }

        [PrimaryKey]
        public string Id { get; set; }
        public DateTime FollowTime { get; private set; }
        public DateTime? UnFollowTime { get; set; }
    }
}