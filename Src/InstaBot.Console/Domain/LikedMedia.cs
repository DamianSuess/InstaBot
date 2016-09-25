using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace InstaBot.Console.Domain
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
