using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broadcaster
{
    public interface IBroadcaster
    {
        TEvent Event<TEvent>() where TEvent : IBroadcastEvent, new();
    }
}
