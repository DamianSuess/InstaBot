namespace InstaBot.InstagramAPI.Event
{
    public class AfterFollowEvent : InstagramBaseEvent<string>
    {
        public AfterFollowEvent(object sender, string userId) : base(sender, userId)
        {
        }
    }
}