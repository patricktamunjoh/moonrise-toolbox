using System;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Messaging;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoonriseGames.Toolbox.Tests.Messaging
{
    public class MessagingServiceTest
    {
        [SetUp]
        public void Setup() => Function.ClearScene();

        [Test]
        public void ShouldNotifyForAllMessages()
        {
            var message01 = new SampleMessage();
            var message02 = new SampleBaseMessage();

            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe(subscriber, x => messages.Add(x));
            sut.Send(message01);
            sut.Send(message02);

            Assert.True(messages.SequenceEqual(new[] { message01, message02 }));
        }

        [Test]
        public void ShouldNotifyForTargetMessage()
        {
            var messages = 0;
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, () => messages++);
            sut.Send(new SampleMessage());
            sut.Send(new SampleMessage());

            Assert.AreEqual(2, messages);
        }

        [Test]
        public void ShouldNotifyAndProvideTargetMessage()
        {
            var message = new SampleMessage();

            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, x => messages.Add(x));
            sut.Send(message);
            sut.Send(message);

            Assert.True(messages.SequenceEqual(new[] { message, message }));
        }

        [Test]
        public void ShouldNotNotifyForParentMessage()
        {
            var messages = 0;
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, () => messages++);
            sut.Send(new SampleBaseMessage());
            sut.Send(new SampleBaseMessage());

            Assert.AreEqual(0, messages);
        }

        [Test]
        public void ShouldNotNotifyAndProvideParentMessage()
        {
            var message = new SampleBaseMessage();

            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, x => messages.Add(x));
            sut.Send(message);
            sut.Send(message);

            Assert.IsEmpty(messages);
        }

        [Test]
        public void ShouldNotifyForChildMessage()
        {
            var messages = 0;
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleBaseMessage>(subscriber, () => messages++);
            sut.Send(new SampleMessage());
            sut.Send(new SampleMessage());

            Assert.AreEqual(2, messages);
        }

        [Test]
        public void ShouldNotifyAndProvideChildMessage()
        {
            var message = new SampleMessage();

            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleBaseMessage>(subscriber, x => messages.Add(x));
            sut.Send(message);
            sut.Send(message);

            Assert.True(messages.SequenceEqual(new[] { message, message }));
        }

        [Test]
        public void ShouldFlushWithNullMessage()
        {
            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe(subscriber, x => messages.Add(x));
            sut.Flush();
            sut.Flush();

            Assert.True(messages.SequenceEqual(new Message[] { null, null }));
        }

        [Test]
        public void ShouldNotNotifyForNullMessage()
        {
            var messages = 0;
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleBaseMessage>(subscriber, () => messages++);
            sut.Flush();

            Assert.AreEqual(0, messages);
        }

        [Test]
        public void ShouldNotifyAndProvideNullMessage()
        {
            var messages = new List<Message>();
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleBaseMessage>(subscriber, x => messages.Add(x));
            sut.Flush();

            Assert.IsEmpty(messages);
        }

        [Test]
        public void ShouldRemoveInvalidSubscriptions()
        {
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, () => Assert.True(false));

            Object.DestroyImmediate(subscriber);
            sut.Send(new SampleMessage());
        }

        [Test]
        public void ShouldHandleNullSubscribers()
        {
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(null, () => Assert.True(false));
            sut.Send(new SampleMessage());
        }

        [Test]
        public void ShouldHandleNullCallbacks()
        {
            var subscriber = new GameObject().AddComponent<SampleBehaviour>();
            var sut = new GameObject().AddComponent<MessagingService>();

            sut.Subscribe<SampleMessage>(subscriber, null as Action);
            sut.Send(new SampleMessage());
        }

        private class SampleBaseMessage : Message { }

        private class SampleMessage : SampleBaseMessage { }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
