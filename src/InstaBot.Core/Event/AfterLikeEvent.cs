namespace InstaBot.Core.Event
{
    public class AfterLikeEvent : InstagramBaseEvent<string>
    {
        public AfterLikeEvent(object sender, string mediaId) : base(sender, mediaId)
        {
        }
    }
}