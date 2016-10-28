using System;
using ServiceStack.DataAnnotations;

namespace InstaBot.Core.Domain
{
    public class LikedMedia : EntityBase<string>
    {
        public LikedMedia(string id) : base(id)
        {
            CreationTime = DateTime.Now;
        }
        
        public DateTime CreationTime { get; private set; }
    }
}
