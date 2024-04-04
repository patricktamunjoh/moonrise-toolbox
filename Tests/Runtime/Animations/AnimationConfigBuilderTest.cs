using MoonriseGames.Toolbox.Animations;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimationConfigBuilderTest
    {
        [Test]
        public void ShouldBuildWithReasonableDefaultValues()
        {
            var config = new AnimationConfigBuilder().Build();

            Assert.False(config.UseUnscaledTime);
            Assert.AreEqual(AnimationPlayback.Forward, config.Playback);
            Assert.AreEqual(AnimationInterpolation.Linear, config.Interpolation);
        }

        [Test]
        public void ShouldBuildWithConfiguredValues()
        {
            var config = new AnimationConfigBuilder()
                .WithUnscaledTime()
                .WithPlayback(AnimationPlayback.PingPong)
                .WithInterpolation(AnimationInterpolation.EaseInOut)
                .Build();

            Assert.True(config.UseUnscaledTime);
            Assert.AreEqual(AnimationPlayback.PingPong, config.Playback);
            Assert.AreEqual(AnimationInterpolation.EaseInOut, config.Interpolation);
        }
    }
}
