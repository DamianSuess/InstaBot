using System.ComponentModel;

namespace InstaBot.Console.Settings
{
    public class BotSettings : BaseSettings
    {
        public BotSettings()
        {
            Tags = new string[] { "car" };
            StopTags = new string[] { "hot", "money" };
        }

        public string[] Tags { get; set; }
        public string[] StopTags { get; set; }

        [DefaultValue(350)]
        public int MaxLikePerHour { get; set; }
        [DefaultValue(1000)]
        public int MaxLikePerDay { get; set; }
        [DefaultValue(1)]
        public int MinLikeToLike { get; set; }
        [DefaultValue(50)]
        public int MaxLikeToLike { get; set; }
        [DefaultValue(350)]
        public int MaxFollowPerDay { get; set; }
        [DefaultValue(350)]
        public int UnFollowPerDay { get; set; }
        [DefaultValue(350)]
        public int CommentPerDay { get; set; }
        [DefaultValue(null)]
        public int? LikeRatio { get; set; }
        [DefaultValue(0.90)]
        public double FollowingRatio { get; set; }
    }
}