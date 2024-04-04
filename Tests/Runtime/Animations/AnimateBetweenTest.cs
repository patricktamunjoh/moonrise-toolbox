using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateBetweenTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldInvokeOnProgressWithZero()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), p => Assert.AreEqual(0, p));
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnProgressWithOne()
        {
            var sut = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), p => Assert.AreEqual(1, p));
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnProgressBackwardsWhenReversed()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), p => Assert.AreEqual(1, p));

            sut.SetDirection(true);
            sut.Start();
        }

        [Test]
        public void ShouldHandleNullAction()
        {
            var sut = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnComplete()
        {
            var isInvoked = false;
            var sut = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldStartOnlyOnceWithForwardOnce()
        {
            var invocations = 0;
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.ForwardOnce).Build();
            var sut = new AnimateBetween(Context, 0, config, null);

            sut.OnComplete += () => invocations++;
            sut.Start();
            sut.Start();

            Assert.AreEqual(1, invocations);
        }

        [Test]
        public void ShouldRestartOnlyOnceWithForwardOnce()
        {
            var invocations = 0;
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.ForwardOnce).Build();
            var sut = new AnimateBetween(Context, 0, config, null);

            sut.OnComplete += () => invocations++;
            sut.Restart();
            sut.Restart();

            Assert.AreEqual(1, invocations);
        }

        [Test]
        public void ShouldResetToZero()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), p => Assert.AreEqual(0, p));
            sut.Reset();
        }

        [Test]
        public void ShouldResetToOneWhenReversed()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), p => Assert.AreEqual(1, p));

            sut.SetDirection(true);
            sut.Reset();
        }

        [Test]
        public void ShouldReportIsStartedStatus()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);

            Assert.False(sut.IsStarted);

            sut.Start();
            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldReportIsPausedStatus()
        {
            var sut = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);

            Assert.False(sut.IsPaused);

            sut.Start();
            sut.Pause();

            Assert.True(sut.IsPaused);

            sut.Resume();

            Assert.False(sut.IsPaused);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
