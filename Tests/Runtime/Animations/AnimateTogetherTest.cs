using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateTogetherTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldStartAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Start();

            animate01.Verify(x => x.Start(), Times.Once);
            animate02.Verify(x => x.Start(), Times.Once);
        }

        [Test]
        public void ShouldStopAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Start();
            sut.Stop();

            animate01.Verify(x => x.Stop(), Times.Once);
            animate02.Verify(x => x.Stop(), Times.Once);
        }

        [Test]
        public void ShouldPauseAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Start();
            sut.Pause();

            animate01.Verify(x => x.Pause(), Times.Once);
            animate02.Verify(x => x.Pause(), Times.Once);
        }

        [Test]
        public void ShouldResumeAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Start();
            sut.Pause();
            sut.Resume();

            animate01.Verify(x => x.Resume(), Times.Once);
            animate02.Verify(x => x.Resume(), Times.Once);
        }

        [Test]
        public void ShouldRestartAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Restart();

            animate01.Verify(x => x.Restart(), Times.Once);
            animate02.Verify(x => x.Restart(), Times.Once);
        }

        [Test]
        public void ShouldResetAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.Reset();

            animate01.Verify(x => x.Reset(), Times.Once);
            animate02.Verify(x => x.Reset(), Times.Once);
        }

        [Test]
        public void ShouldInvokeOnCompleteWhenAllAnimationsAreComplete()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteWhenStartedWithNoAnimations()
        {
            var isInvoked = false;
            var sut = new AnimateTogether();

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteWhenRestartedWithNoAnimations()
        {
            var isInvoked = false;
            var sut = new AnimateTogether();

            sut.OnComplete += () => isInvoked = true;
            sut.Restart();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteForEndlessAnimations()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, null);
            var sut = new AnimateTogether(animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteForPartialCompletion()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteBeforeStart()
        {
            var isInvoked = false;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate);

            sut.OnComplete += () => isInvoked = true;
            animate.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteAfterStop()
        {
            var isInvoked = false;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animateMock = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate, animateMock.Object);

            animateMock.SetupGet(x => x.IsStarted).Returns(true);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();
            sut.Stop();

            animateMock.SetupGet(x => x.IsStarted).Returns(false);
            animate.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnCompleteAfterReset()
        {
            var isInvoked = false;
            var animate = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animateMock = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate, animateMock.Object);

            animateMock.SetupGet(x => x.IsStarted).Returns(true);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();
            sut.Reset();

            animateMock.SetupGet(x => x.IsStarted).Returns(false);
            animate.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnCompleteOnlyOncePerStart()
        {
            var invocations = 0;
            var animate01 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            sut.OnComplete += () => invocations++;
            sut.Start();
            sut.Restart();

            animate01.Start();
            animate02.Start();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldBeStartedWhenAnyAnimationIsStarted()
        {
            var animate01 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            animate01.Start();

            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBeStartedWhenNoAnimationIsStarted()
        {
            var animate01 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldBePausedWhenAnyAnimationIsPausedAndOthersAreNotRunning()
        {
            var animate01 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            animate01.Start();
            animate01.Pause();

            Assert.True(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedWhenNoAnimationIsStarted()
        {
            var animate01 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedWhenAnyAnimationIsRunning()
        {
            var animate01 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 1, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateTogether(animate01, animate02);

            animate02.Start();
            animate01.Start();
            animate01.Pause();

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotSetDirectionInitially()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            animate01.Verify(x => x.SetDirection(It.IsAny<bool>()), Times.Never);
            animate02.Verify(x => x.SetDirection(It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void ShouldSetDirectionOnAllAnimations()
        {
            var animate01 = new Mock<IAnimateDirectional>();
            var animate02 = new Mock<IAnimateDirectional>();
            var sut = new AnimateTogether(animate01.Object, animate02.Object);

            sut.SetDirection(true);

            animate01.Verify(x => x.SetDirection(true), Times.Once);
            animate02.Verify(x => x.SetDirection(true), Times.Once);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
