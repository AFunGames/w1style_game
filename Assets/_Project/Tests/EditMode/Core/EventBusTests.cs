using System;
using NUnit.Framework;
using W1Style.Infrastructure.Services;

namespace W1Style.Tests.EditMode.Core
{
    /// <summary>
    /// Unit tests for the EventBus implementation.
    /// Pure C# tests — no Unity scene required.
    /// </summary>
    [TestFixture]
    public sealed class EventBusTests
    {
        private struct TestEvent
        {
            public int Value;
        }

        private struct OtherEvent
        {
            public string Message;
        }

        private EventBus _eventBus;

        [SetUp]
        public void SetUp()
        {
            _eventBus = new EventBus();
        }

        [TearDown]
        public void TearDown()
        {
            _eventBus.Clear();
        }

        [Test]
        public void Publish_WithSubscriber_InvokesHandler()
        {
            var received = false;
            _eventBus.Subscribe<TestEvent>(_ => received = true);

            _eventBus.Publish(new TestEvent { Value = 42 });

            Assert.IsTrue(received);
        }

        [Test]
        public void Publish_WithSubscriber_PassesCorrectData()
        {
            var receivedValue = 0;
            _eventBus.Subscribe<TestEvent>(e => receivedValue = e.Value);

            _eventBus.Publish(new TestEvent { Value = 7 });

            Assert.AreEqual(7, receivedValue);
        }

        [Test]
        public void Publish_WithNoSubscribers_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _eventBus.Publish(new TestEvent { Value = 1 }));
        }

        [Test]
        public void Unsubscribe_RemovesHandler()
        {
            var callCount = 0;
            Action<TestEvent> handler = _ => callCount++;

            _eventBus.Subscribe(handler);
            _eventBus.Publish(new TestEvent());
            _eventBus.Unsubscribe(handler);
            _eventBus.Publish(new TestEvent());

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Subscribe_MultipleHandlers_AllInvoked()
        {
            var count1 = 0;
            var count2 = 0;

            _eventBus.Subscribe<TestEvent>(_ => count1++);
            _eventBus.Subscribe<TestEvent>(_ => count2++);

            _eventBus.Publish(new TestEvent());

            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
        }

        [Test]
        public void Publish_DifferentEventTypes_DoNotInterfere()
        {
            var testReceived = false;
            var otherReceived = false;

            _eventBus.Subscribe<TestEvent>(_ => testReceived = true);
            _eventBus.Subscribe<OtherEvent>(_ => otherReceived = true);

            _eventBus.Publish(new TestEvent());

            Assert.IsTrue(testReceived);
            Assert.IsFalse(otherReceived);
        }

        [Test]
        public void Clear_RemovesAllHandlers()
        {
            var received = false;
            _eventBus.Subscribe<TestEvent>(_ => received = true);

            _eventBus.Clear();
            _eventBus.Publish(new TestEvent());

            Assert.IsFalse(received);
        }
    }
}
