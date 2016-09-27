using TinyMessenger;

namespace InstaBot.Core.Event
{
    public abstract class InstagramBaseEvent : TinyMessageBase
    {
        protected InstagramBaseEvent(object sender) : base(sender)
        {

        }
    }

    public class InstagramBaseEvent<T> : InstagramBaseEvent
    {
        public T Entity { get; set; }
        public InstagramBaseEvent(object sender, T entity) : base(sender)
        {
            Entity = entity;
        }
    }
}
