using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broadcaster
{
    public abstract class BroadcasterEvent<TInput> : IBroadcastEvent
    {
        public event EventHandler OnLastUnsubscribed;

        protected readonly List<Action<TInput>> _subscriptions;

        public List<Action<TInput>> Subscriptions
        {
            get { return _subscriptions; }
        }

        public BroadcasterEvent()
        {
            _subscriptions = new List<Action<TInput>>();
        }

        public void Subscribe(Action<TInput> action)
        {
            if (!_subscriptions.Contains(action))
                _subscriptions.Add(action);
        }

        public void Unsubscribe(Action<TInput> action)
        {
            if (!_subscriptions.Contains(action))
                return;

            _subscriptions.Remove(action);

            if (_subscriptions.Count == 0 && OnLastUnsubscribed != null)
                OnLastUnsubscribed(this, new EventArgs());
        }

        public void Broadcast(TInput message)
        {
            Broadcast(message, false);
        }

        public void Broadcast(TInput message, bool throwWithoutSubscribers)
        {
            if (_subscriptions != null && _subscriptions.Any())
            {
                foreach (var action in _subscriptions)
                    action(message);
            }
            else if (throwWithoutSubscribers)
                throw new Exception("Subscription not found");
        }
    }
}
