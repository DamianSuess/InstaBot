using System;
using ServiceStack.DataAnnotations;

namespace InstaBot.Core.Domain
{
    public class LikedMedia
    {
        public LikedMedia(string id)
        {
            Id = id;
            CreationTime = DateTime.Now;
        }

        [PrimaryKey]
        public string Id { get; set; }
        public DateTime CreationTime { get; private set; }
    }
}
