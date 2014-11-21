using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Broadcaster;

namespace Broadcaster
{
    public class BroadcastContainer : IBroadcaster
    {
        private readonly List<IBroadcastEvent> _events;

        public List<IBroadcastEvent> Events
        {
            get { return _events; }
        }

        public BroadcastContainer()
        {
            _events = new List<IBroadcastEvent>();
        }

        public TEvent Event<TEvent>()
            where TEvent : IBroadcastEvent, new()
        {
            var evt = _events.FirstOrDefault(o => o.GetType() == typeof(TEvent));

            if (evt == null)
            {
                evt = new TEvent();
                evt.OnLastUnsubscribed += OnLastUnsubscribed;
                _events.Add(evt);
            }

            return (TEvent)evt;
        }

        void OnLastUnsubscribed(object sender, EventArgs e)
        {
            var evt = sender as IBroadcastEvent;
            evt.OnLastUnsubscribed -= OnLastUnsubscribed;
            _events.Remove(evt);
        }
    }
}
