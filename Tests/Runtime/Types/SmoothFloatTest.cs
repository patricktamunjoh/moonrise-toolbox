using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Types
{
    public class SmoothFloatTest
    {
        [Test]
        public void ShouldInitializeWithDefaultValues()
        {
            var sut = new SmoothFloat();

            Assert.Zero(sut.Value);
            Assert.Zero(sut.Velocity);
        }

        [Test]
        public void ShouldInitializeWithGivenValues()
        {
            var sut = new SmoothFloat(12, SmoothingDuration.Long);

            Assert.AreEqual(12, sut.Value);
            Assert.Zero(sut.Velocity);
        }

        [Test]
        public void ShouldRespectMaxVelocity()
        {
            var sut = new SmoothFloat(12, SmoothingDuration.Long, 0);
            sut.Advance(15);

            Assert.Zero(sut.Velocity);
        }

        [Test]
        public void ShouldResetToDefaultValues()
        {
            var sut = new SmoothFloat(12, SmoothingDuration.Long, 0);
            sut.Advance(15);
            sut.Reset();

            Assert.Zero(sut.Velocity);
            Assert.Zero(sut.Value);
        }

        [Test]
        public void ShouldReturnCurrentValueOnAdvance()
        {
            var sut = new SmoothFloat(12, SmoothingDuration.Long);

            var value = sut.Advance(15);
            Assert.True(value.IsSimilar(sut.Value));

            value = sut.Advance(20, 15);
            Assert.True(value.IsSimilar(sut.Value));
        }

        [Test]
        public void ShouldAdvanceToGivenTarget()
        {
            var sut = new SmoothFloat(12, 0f, 100);

            for (var i = 0; i < 100; i++) sut.Advance(13);
            Assert.True(sut.Value.IsSimilar(13));
        }

        [Test]
        public void ShouldAdvanceUsingGivenCurrentValue()
        {
            var sut = new SmoothFloat(5, 100f, 100);

            sut.Advance(100, 5);
            Assert.Greater(sut.Value, 10);
        }
    }
}
