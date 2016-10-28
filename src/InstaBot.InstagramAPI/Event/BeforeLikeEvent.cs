using InstaBot.InstagramAPI.Domain;

namespace InstaBot.InstagramAPI.Event
{
    public class BeforeLikeEvent : InstagramBaseEvent<Media>
    {
        public BeforeLikeEvent(object sender, Media media) : base(sender, media)
        {
        }
    }
}