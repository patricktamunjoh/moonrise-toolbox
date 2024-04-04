using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateContinuousTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldInvokeOnProgressWithPositiveValue()
        {
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, Assert.Positive);
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnProgressWithNegativeValueWhenReversed()
        {
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, Assert.Negative);
            sut.SetDirection(true);
            sut.Start();
        }

        [Test]
        public void ShouldHandleNullAction()
        {
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, null);
            sut.Start();
            sut.Reset();
        }

        [Test]
        public void ShouldInvokeOnReset()
        {
            var isInvoked = false;
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), () => isInvoked = true, null);

            sut.Start();
            sut.Reset();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldReportIsStartedStatus()
        {
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, null);

            Assert.False(sut.IsStarted);

            sut.Start();
            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldReportIsPausedStatus()
        {
            var sut = new AnimateContinuous(Context, new AnimationConfigBuilder().Build(), null, null);

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
