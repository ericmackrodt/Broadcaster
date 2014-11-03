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
            broadcaster = new Broadcaster();
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_To_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(1, b.Subscriptions.Count);
            Assert.AreEqual(typeof(DummyMessageOne), b.Subscriptions[0].ChannelType);
            Assert.AreEqual(methodOne, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);

            broadcaster.Subscribe<DummyMessageTwo>(methodTwo);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(2, b.Subscriptions.Count);
            Assert.AreEqual(typeof(DummyMessageOne), b.Subscriptions[0].ChannelType);
            Assert.AreEqual(typeof(DummyMessageTwo), b.Subscriptions[1].ChannelType);
            Assert.AreEqual(methodOne, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
            Assert.AreEqual(methodTwo, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions[0]);
        }

        [TestMethod]
        public void Should_Have_One_Channel_Subscribing_To_The_Same_Message_Multiple_Times()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);

            broadcaster.Subscribe<DummyMessageOne>(methodTwo);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(1, b.Subscriptions.Count);
            Assert.AreEqual(2, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions.Count);
            Assert.AreEqual(methodOne, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
            Assert.AreEqual(methodTwo, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[1]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Subscribe_Multiple_Times_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });
            var methodThree = new Action<DummyMessageTwo>((m) => { });
            var methodFour = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);

            broadcaster.Subscribe<DummyMessageOne>(methodTwo);

            broadcaster.Subscribe<DummyMessageTwo>(methodThree);

            broadcaster.Subscribe<DummyMessageTwo>(methodFour);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(2, b.Subscriptions.Count);
            Assert.AreEqual(2, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions.Count);
            Assert.AreEqual(2, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions.Count);
            Assert.AreEqual(methodOne, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
            Assert.AreEqual(methodTwo, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[1]);
            Assert.AreEqual(methodThree, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions[0]);
            Assert.AreEqual(methodFour, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions[1]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_From_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Unsubscribe(methodOne);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(0, b.Subscriptions.Count);
        }

        [TestMethod]
        public void Should_Unsubscribe_Only_From_One_When_Multiple_Subscriptions_On_The_Same_Message()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageOne>(methodTwo);
            broadcaster.Unsubscribe(methodOne);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(1, b.Subscriptions.Count);
            Assert.AreEqual(1, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions.Count);
            Assert.AreEqual(methodTwo, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_When_Subscribed_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageTwo>(methodTwo);
            broadcaster.Unsubscribe(methodOne);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(1, b.Subscriptions.Count);
            Assert.AreEqual(1, (b.Subscriptions[0] as Subscription<DummyMessageTwo>).Actions.Count);
            Assert.AreEqual(methodTwo, (b.Subscriptions[0] as Subscription<DummyMessageTwo>).Actions[0]);
        }

        [TestMethod]
        public void Should_Be_Able_To_Unsubscribe_When_Subscribed_Multiple_Times_To_Multiple_Messages()
        {
            var methodOne = new Action<DummyMessageOne>((m) => { });
            var methodTwo = new Action<DummyMessageOne>((m) => { });
            var methodThree = new Action<DummyMessageTwo>((m) => { });
            var methodFour = new Action<DummyMessageTwo>((m) => { });

            broadcaster.Subscribe<DummyMessageOne>(methodOne);

            broadcaster.Subscribe<DummyMessageOne>(methodTwo);

            broadcaster.Subscribe<DummyMessageTwo>(methodThree);

            broadcaster.Subscribe<DummyMessageTwo>(methodFour);

            broadcaster.Unsubscribe(methodOne);
            broadcaster.Unsubscribe(methodThree);

            var b = (broadcaster as Broadcaster);

            Assert.AreEqual(2, b.Subscriptions.Count);
            Assert.AreEqual(1, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions.Count);
            Assert.AreEqual(1, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions.Count);
            Assert.AreEqual(methodTwo, (b.Subscriptions[0] as Subscription<DummyMessageOne>).Actions[0]);
            Assert.AreEqual(methodFour, (b.Subscriptions[1] as Subscription<DummyMessageTwo>).Actions[0]);
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

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Broadcast(new DummyMessageOne() { Content = message });

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

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageOne>(methodTwo);
            broadcaster.Broadcast(new DummyMessageOne() { Content = message });

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

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageTwo>(methodTwo);
            broadcaster.Broadcast(new DummyMessageOne() { Content = message });

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

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageOne>(methodTwo);
            broadcaster.Subscribe<DummyMessageTwo>(methodThree);
            broadcaster.Subscribe<DummyMessageTwo>(methodFour);
            broadcaster.Broadcast(new DummyMessageOne() { Content = message });

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

            broadcaster.Subscribe<DummyMessageOne>(methodOne);
            broadcaster.Subscribe<DummyMessageOne>(methodTwo);
            broadcaster.Subscribe<DummyMessageTwo>(methodThree);
            broadcaster.Subscribe<DummyMessageTwo>(methodFour);
            broadcaster.Broadcast(new DummyMessageOne() { Content = messageOne });
            broadcaster.Broadcast(new DummyMessageTwo() { Content = messageTwo });

            Assert.AreEqual(messageOne, resultOne);
            Assert.AreEqual(messageOne, resultTwo);
            Assert.AreEqual(messageTwo, resultThree);
            Assert.AreEqual(messageTwo, resultFour);
        }
    }
}
