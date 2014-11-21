using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Broadcaster.Tests
{
    [TestClass]
    public class BroadcasterTests
    {
        IBroadcaster broadcaster;

        [TestInitialize]
        public void Initialize()
        {
            broadcaster = new BroadcastContainer();
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_To_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(1, b.Events.Count);
            Assert.AreEqual(typeof(DummyEventOne), b.Events[0].GetType());
            Assert.AreEqual(methodOne, (b.Events[0] as DummyEventOne).Subscriptions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);

            broadcaster.Event<DummyEventTwo>().Subscribe(methodTwo);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(2, b.Events.Count);
            Assert.AreEqual(typeof(DummyEventOne), b.Events[0].GetType());
            Assert.AreEqual(typeof(DummyEventTwo), b.Events[1].GetType());
            Assert.AreEqual(methodOne, (b.Events[0] as DummyEventOne).Subscriptions[0]);
            Assert.AreEqual(methodTwo, (b.Events[1] as DummyEventTwo).Subscriptions[0]);
        }

        [TestMethod]
        public void Should_Have_One_Channel_Subscribing_To_The_Same_Message_Multiple_Times()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);

            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(1, b.Events.Count);
            Assert.AreEqual(2, (b.Events[0] as DummyEventOne).Subscriptions.Count);
            Assert.AreEqual(methodOne, (b.Events[0] as DummyEventOne).Subscriptions[0]);
            Assert.AreEqual(methodTwo, (b.Events[0] as DummyEventOne).Subscriptions[1]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_Multiple_Times_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });
            var methodThree = new Action<DummyMessageTwo>((m) => { });
            var methodFour = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodThree);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodFour);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(2, b.Events.Count);
            Assert.AreEqual(2, (b.Events[0] as DummyEventOne).Subscriptions.Count);
            Assert.AreEqual(2, (b.Events[1] as DummyEventTwo).Subscriptions.Count);
            Assert.AreEqual(methodOne, (b.Events[0] as DummyEventOne).Subscriptions[0]);
            Assert.AreEqual(methodTwo, (b.Events[0] as DummyEventOne).Subscriptions[1]);
            Assert.AreEqual(methodThree, (b.Events[1] as DummyEventTwo).Subscriptions[0]);
            Assert.AreEqual(methodFour, (b.Events[1] as DummyEventTwo).Subscriptions[1]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_From_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Unsubscribe(methodOne);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(0, b.Events.Count);
        }

        [TestMethod]
        public void Should_Unsubscribe_Only_From_One_When_Multiple_Subscriptions_On_The_Same_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventOne>().Unsubscribe(methodOne);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(1, b.Events.Count);
            Assert.AreEqual(1, (b.Events[0] as DummyEventOne).Subscriptions.Count);
            Assert.AreEqual(methodTwo, (b.Events[0] as DummyEventOne).Subscriptions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_When_Subscribed_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventOne>().Unsubscribe(methodOne);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(1, b.Events.Count);
            Assert.AreEqual(1, (b.Events[0] as DummyEventTwo).Subscriptions.Count);
            Assert.AreEqual(methodTwo, (b.Events[0] as DummyEventTwo).Subscriptions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_When_Subscribed_Multiple_Times_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });
            var methodThree = new Action<DummyMessageTwo>((m) => { });
            var methodFour = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodThree);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodFour);
            broadcaster.Event<DummyEventOne>().Unsubscribe(methodOne);
            broadcaster.Event<DummyEventTwo>().Unsubscribe(methodThree);

            var b = (broadcaster as BroadcastContainer);

            Assert.AreEqual(2, b.Events.Count);
            Assert.AreEqual(1, (b.Events[0] as DummyEventOne).Subscriptions.Count);
            Assert.AreEqual(1, (b.Events[1] as DummyEventTwo).Subscriptions.Count);
            Assert.AreEqual(methodTwo, (b.Events[0] as DummyEventOne).Subscriptions[0]);
            Assert.AreEqual(methodFour, (b.Events[1] as DummyEventTwo).Subscriptions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Broadcast_A_Message()
        {
            var message = "message";
            var result = "";

            var methodOne = new Action<DummyMessageOne>((m) =>
            {
                result = m.Content;
            });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Broadcast(new DummyMessageOne() { Content = message });

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void Should_Be_Able_To_Broadcast_To_Multiple_Subscriptions()
        {
            var message = "message";
            var resultOne = "";
            var resultTwo = "";

            var methodOne = new Action<DummyMessageOne>((m) =>
            {
                resultOne = m.Content;
            });

            var methodTwo = new Action<DummyMessageOne>((m) =>
            {
                resultTwo = m.Content;
            });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventOne>().Broadcast(new DummyMessageOne() { Content = message });

            Assert.AreEqual(message, resultOne);
            Assert.AreEqual(message, resultTwo);
        }

        [TestMethod]
        public void Should_Broadcast_Message_To_Only_One_Channel()
        {
            var message = "message";
            var resultOne = "";
            var resultTwo = "";

            var methodOne = new Action<DummyMessageOne>((m) =>
            {
                resultOne = m.Content;
            });

            var methodTwo = new Action<DummyMessageTwo>((m) =>
            {
                resultTwo = m.Content;
            });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventOne>().Broadcast(new DummyMessageOne() { Content = message });

            Assert.AreEqual(message, resultOne);
            Assert.AreEqual("", resultTwo);
        }

        [TestMethod]
        public void Should_Broadcast_Message_To_Only_One_Channel_When_Multiple_Subscriptions()
        {
            var message = "message";
            var resultOne = "";
            var resultTwo = "";
            var resultThree = "";
            var resultFour = "";

            var methodOne = new Action<DummyMessageOne>((m) =>
            {
                resultOne = m.Content;
            });

            var methodTwo = new Action<DummyMessageOne>((m) =>
            {
                resultTwo = m.Content;
            });

            var methodThree = new Action<DummyMessageTwo>((m) =>
            {
                resultThree = m.Content;
            });

            var methodFour = new Action<DummyMessageTwo>((m) =>
            {
                resultFour = m.Content;
            });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodThree);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodFour);
            broadcaster.Event<DummyEventOne>().Broadcast(new DummyMessageOne() { Content = message });

            Assert.AreEqual(message, resultOne);
            Assert.AreEqual(message, resultTwo);
            Assert.AreEqual("", resultThree);
            Assert.AreEqual("", resultFour);
        }

        [TestMethod]
        public void Should_Be_Able_To_Broadcast_Different_Messages_To_Different_Channels()
        {
            var messageOne = "messageOne";
            var messageTwo = "messageTwo";
            var resultOne = "";
            var resultTwo = "";
            var resultThree = "";
            var resultFour = "";

            var methodOne = new Action<DummyMessageOne>((m) =>
            {
                resultOne = m.Content;
            });

            var methodTwo = new Action<DummyMessageOne>((m) =>
            {
                resultTwo = m.Content;
            });

            var methodThree = new Action<DummyMessageTwo>((m) =>
            {
                resultThree = m.Content;
            });

            var methodFour = new Action<DummyMessageTwo>((m) =>
            {
                resultFour = m.Content;
            });

            broadcaster.Event<DummyEventOne>().Subscribe(methodOne);
            broadcaster.Event<DummyEventOne>().Subscribe(methodTwo);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodThree);
            broadcaster.Event<DummyEventTwo>().Subscribe(methodFour);
            broadcaster.Event<DummyEventOne>().Broadcast(new DummyMessageOne() { Content = messageOne });
            broadcaster.Event<DummyEventTwo>().Broadcast(new DummyMessageTwo() { Content = messageTwo });

            Assert.AreEqual(messageOne, resultOne);
            Assert.AreEqual(messageOne, resultTwo);
            Assert.AreEqual(messageTwo, resultThree);
            Assert.AreEqual(messageTwo, resultFour);
        }
    }
}
