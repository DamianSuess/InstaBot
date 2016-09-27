namespace InstaBot.Core.Event
{
    public class BeforeLikeEvent : InstagramBaseEvent<string>
    {
        public BeforeLikeEvent(object sender, string mediaId) : base(sender, mediaId)
        {
        }
    }
}