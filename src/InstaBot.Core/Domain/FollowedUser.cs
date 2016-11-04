using System;

namespace InstaBot.Core.Domain
{
    public class FollowedUser : EntityBase<string>
    {
        public FollowedUser(string id) : base(id)
        {
            FollowTime = DateTime.Now;
            UnFollowTime = null;
            IgnoreTime = null;
        }
        
        public DateTime FollowTime { get; set; }
        public DateTime? UnFollowTime { get; set; }
        public DateTime? IgnoreTime { get; set; }
    }
}