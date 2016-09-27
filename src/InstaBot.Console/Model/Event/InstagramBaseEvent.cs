using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace InstaBot.Console.Model.Event
{
    public abstract class InstagramBaseEvent : TinyMessageBase
    {
        public InstagramBaseEvent(object sender): base(sender)
    {

        }
    }
    public class BeforeLikeEvent : InstagramBaseEvent
    {
        public BeforeLikeEvent(object sender) : base(sender)
        {
        }
    }
    public class AfterLikeEvent : InstagramBaseEvent
    {
        public AfterLikeEvent(object sender) : base(sender)
        {
        }
    }
    public class BeforeFollowEvent : InstagramBaseEvent
    {
        public BeforeFollowEvent(object sender) : base(sender)
        {
        }
    }
    public class AfterFollowEvent : InstagramBaseEvent
    {
        public AfterFollowEvent(object sender) : base(sender)
        {
        }
    }
}
