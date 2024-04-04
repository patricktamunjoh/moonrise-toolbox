using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateSequenceTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldBePausedWhenCurrentAnimationIsPaused()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            animate.SetupGet(x => x.IsPaused).Returns(true);
            sut.Start();

            Assert.True(sut.IsPaused);
        }

        [Test]
        public void ShouldBeStartedWhenCurrentAnimationIsStarted()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            animate.SetupGet(x => x.IsStarted).Returns(true);
            sut.Start();

            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBePausedWithoutAnimations()
        {
            var sut = new AnimateSequence();

            sut.Start();
            sut.Pause();

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBeStartedWithoutAnimations()
        {
            var sut = new AnimateSequence();
            sut.Start();

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBePausedBeforeStart()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            animate.SetupGet(x => x.IsPaused).Returns(true);

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBeStartedBeforeStart()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            animate.SetupGet(x => x.IsStarted).Returns(true);

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBeStartedAfterStop()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            animate.SetupGet(x => x.IsStarted).Returns(true);

            sut.Start();
            sut.Stop();

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldInvokeOnCompleteAfterAllAnimationsWhenPlayingForward()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(AnimationPlayback.Forward, animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteAfterAllAnimationsWhenPlayingForwardOnce()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(AnimationPlayback.ForwardOnce, animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteWhenStartedWithoutAnimations()
        {
            var isInvoked = false;
            var sut = new AnimateSequence(AnimationPlayback.ForwardOnce);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteWhenRestartedWithoutAnimations()
        {
            var isInvoked = false;
            var sut = new AnimateSequence(AnimationPlayback.Forward);

            sut.OnComplete += () => isInvoked = true;
            sut.Restart();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteForLoop()
        {
            var isInvoked = false;
            var sut = new AnimateSequence(AnimationPlayback.Loop);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteForPingPong()
        {
            var isInvoked = false;
            var sut = new AnimateSequence(AnimationPlayback.PingPong);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteOnlyOncePerStart()
        {
            var invocations = 0;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate);

            sut.OnComplete += () => invocations++;
            sut.Start();
            animate.Start();

            Assert.AreEqual(1, invocations);
        }

        [Test]
        public void ShouldInvokeOnCompleteOnlyOncePerRestart()
        {
            var invocations = 0;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate);

            sut.OnComplete += () => invocations++;
            sut.Restart();
            animate.Start();

            Assert.AreEqual(1, invocations);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteWhenNotStarted()
        {
            var isInvoked = false;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate);

            sut.OnComplete += () => isInvoked = true;
            animate.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldPlayAnimationsInOrder()
        {
            var startIndices = new List<int>();
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(0));
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(1));
            var sut = new AnimateSequence(animate01, animate02);

            sut.Start();

            Assert.True(startIndices.SequenceEqual(new[] { 0, 1 }));
        }

        [Test]
        public void ShouldStartAtFirstAnimationAfterCompletion()
        {
            var startIndices = new List<int>();
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(0));
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(1));
            var sut = new AnimateSequence(animate01, animate02);

            sut.Start();
            sut.Start();
            sut.Start();

            Assert.True(startIndices.SequenceEqual(new[] { 0, 1, 0, 1, 0, 1 }));
        }

        [Test]
        public void ShouldStartAtFirstAnimationAfterStop()
        {
            var startIndices = new List<int>();
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(0));
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), _ => startIndices.Add(1));
            var sut = new AnimateSequence(animate01, animate02);

            sut.Start();
            sut.Stop();
            sut.Start();

            Assert.True(startIndices.SequenceEqual(new[] { 0, 1, 0, 1 }));
        }

        [Test]
        public void ShouldStartAtFirstAnimationOnRestart()
        {
            var startIndices = new List<int>();
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), _ => startIndices.Add(0));
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), _ => startIndices.Add(1));
            var sut = new AnimateSequence(animate01, animate02);

            sut.Restart();
            sut.Restart();

            Assert.True(startIndices.SequenceEqual(new[] { 0, 1, 0, 1 }));
        }

        [Test]
        public void ShouldStartOnlyOnceWithForwardOnce()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(AnimationPlayback.ForwardOnce, animate.Object);

            sut.Start();
            sut.Start();

            animate.Verify(x => x.Start(), Times.Once);
        }

        [Test]
        public void ShouldRestartOnlyOnceWithForwardOnce()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(AnimationPlayback.ForwardOnce, animate.Object);

            sut.Restart();
            sut.Restart();

            animate.Verify(x => x.Start(), Times.Once);
        }

        [Test]
        public void ShouldPauseAndResumeCurrentAnimation()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            sut.Start();
            sut.Pause();
            sut.Resume();

            animate.Verify(x => x.Pause(), Times.Once);
            animate.Verify(x => x.Resume(), Times.Once);
        }

        [Test]
        public void ShouldStopCurrentAnimationOnRestart()
        {
            var animateMock = new Mock<IAnimateDirectional>();
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate, animateMock.Object);

            sut.Start();
            sut.Restart();

            animateMock.Verify(x => x.Stop(), Times.Once);
        }

        [Test]
        public void ShouldStopCurrentAnimationOnStop()
        {
            var animateMock = new Mock<IAnimateDirectional>();
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate, animateMock.Object);

            sut.Start();
            sut.Stop();

            animateMock.Verify(x => x.Stop(), Times.Once);
        }

        [Test]
        public void ShouldResetAnimationsInReversedOrder()
        {
            var resetIndices = new List<int>();
            var animate01 = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), () => resetIndices.Add(0), null);
            var animate02 = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), () => resetIndices.Add(1), null);
            var sut = new AnimateSequence(animate01, animate02);

            sut.Reset();

            Assert.True(resetIndices.SequenceEqual(new[] { 1, 0 }));
        }

        [Test]
        public void ShouldNotResetAnimationsOnStop()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            sut.Start();
            sut.Stop();

            animate.Verify(x => x.Reset(), Times.Never);
        }

        [Test]
        public void ShouldNotResetAnimationsOnRestart()
        {
            var animate = new Mock<IAnimateDirectional>();
            var sut = new AnimateSequence(animate.Object);

            sut.Start();
            sut.Restart();

            animate.Verify(x => x.Reset(), Times.Never);
        }

        [Test]
        public void ShouldIgnoreAnimationCompletionsOutOfOrder()
        {
            var animateMock01 = new Mock<IAnimateDirectional>();
            var animateMock02 = new Mock<IAnimateDirectional>();
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animateMock01.Object, animateMock02.Object, animate);

            sut.Start();
            animate.Start();

            animateMock02.Verify(x => x.Start(), Times.Never);
        }

        [Test]
        public void ShouldIgnoreAnimationCompletionsBeforeStart()
        {
            var animateMock = new Mock<IAnimateDirectional>();
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate, animateMock.Object);

            animate.Start();

            animateMock.Verify(x => x.Start(), Times.Never);
        }

        [Test]
        public void ShouldIgnoreAnimationCompletionsAfterCompletion()
        {
            var animateMock = new Mock<IAnimateDirectional>();
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(animate, animateMock.Object);

            sut.Start();
            animate.Start();

            animateMock.Verify(x => x.Start(), Times.Once);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
