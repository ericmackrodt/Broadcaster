using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broadcaster
{
    public interface IBroadcaster
    {
        void Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Action<T> action);
        void Broadcast<T>(T message);
        void Broadcast<T>(T message, bool throwWithoutSubscribers);
    }
}
