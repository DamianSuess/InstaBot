namespace InstaBot.InstagramAPI.Event
{
    public class BeforeFollowEvent : InstagramBaseEvent<string>
    {
        public BeforeFollowEvent(object sender, string userId) : base(sender, userId)
        {
        }
    }
}