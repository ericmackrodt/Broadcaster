using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broadcaster
{
    public interface IBroadcastEvent
    {
        event EventHandler OnLastUnsubscribed;
    }
}
