using MoonriseGames.Toolbox.Types;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Types
{
    public class IntervalTest
    {
        [Test]
        public void ShouldDetermineInsideIncludingEdges()
        {
            var sut = new Interval(-1, 2);

            Assert.True(sut.IsInside(-1));
            Assert.True(sut.IsInside(2));
            Assert.True(sut.IsInside(0));

            Assert.False(sut.IsInside(-1.1f));
            Assert.False(sut.IsInside(2.0001f));
        }

        [Test]
        public void ShouldLerpBetweenBoundaries()
        {
            var sut = new Interval(2, 4);

            Assert.AreEqual(2, sut.Lerp(-1));
            Assert.AreEqual(2, sut.Lerp(0));
            Assert.AreEqual(4, sut.Lerp(1));
            Assert.AreEqual(4, sut.Lerp(3));
            Assert.AreEqual(3, sut.Lerp(0.5f));
        }

        [Test]
        public void ShouldInverseLerpBetweenBoundaries()
        {
            var sut = new Interval(2, 4);

            Assert.AreEqual(0, sut.InverseLerp(2));
            Assert.AreEqual(0, sut.InverseLerp(0));
            Assert.AreEqual(0.5, sut.InverseLerp(3));
            Assert.AreEqual(1, sut.InverseLerp(4));
            Assert.AreEqual(1, sut.InverseLerp(5));
        }
    }
}
