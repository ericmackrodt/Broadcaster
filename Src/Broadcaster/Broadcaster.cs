using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Broadcaster;

namespace Broadcaster
{
    public class Broadcaster : IBroadcaster
    {
        private List<ISubscription> subscriptions;

        public List<ISubscription> Subscriptions
        {
            get { return subscriptions; }
        }

        public Broadcaster()
        {
            subscriptions = new List<ISubscription>();
        }

        public void Subscribe<T>(Action<T> action)
        {
            var channel = subscriptions.FirstOrDefault(o => o.ChannelType == typeof(T)) as Subscription<T>;

            if (channel == null)
            {
                channel = new Subscription<T>();
                subscriptions.Add(channel);
            }

            channel.Actions.Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var channel = subscriptions.FirstOrDefault(o => o.ChannelType == typeof(T)) as Subscription<T>;
            if (channel != null)
            {
                channel.Actions.Remove(action);
                if (channel.Actions.Count == 0)
                    subscriptions.Remove(channel);
            }
        }

        public void Broadcast<T>(T message)
        {
            Broadcast<T>(message, false);
        }

        public void Broadcast<T>(T message, bool throwWithoutSubscribers)
        {
            var channel = subscriptions.FirstOrDefault(o => o.ChannelType == typeof(T)) as Subscription<T>;

            if (channel != null && channel.Actions != null && channel.Actions.Any())
            {
                foreach(var action in channel.Actions)
                    action(message);
            }
            else if (throwWithoutSubscribers)
                throw new Exception("Subscription not found");
        }
    }
}
