namespace InstaBot.Core.Event
{
    public class BeforeFollowEvent : InstagramBaseEvent<string>
    {
        public BeforeFollowEvent(object sender, string userId) : base(sender, userId)
        {
        }
    }
}