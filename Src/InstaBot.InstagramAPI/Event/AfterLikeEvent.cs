using InstaBot.InstagramAPI.Domain;

namespace InstaBot.InstagramAPI.Event
{
    public class AfterLikeEvent : InstagramBaseEvent<Media>
    {
        public AfterLikeEvent(object sender, Media media) : base(sender, media)
        {
        }
    }
}