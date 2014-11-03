using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broadcaster
{
    public class Subscription<T> : ISubscription
    {
        public Subscription()
        {
            ChannelType = typeof(T);
            Actions = new List<Action<T>>();
        }

        public Type ChannelType { get; set; }
        public List<Action<T>> Actions { get; set; }
    }
}
