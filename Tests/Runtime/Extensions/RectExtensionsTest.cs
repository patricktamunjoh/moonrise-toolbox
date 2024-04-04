using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class RectExtensionsTest
    {
        [Test]
        public void ShouldReturnEmptyWhenDividingHorizontallyWithNoSegments()
        {
            var sut = new Rect().DivideHorizontal(0).ToArray();
            Assert.Zero(sut.Length);
        }

        [Test]
        public void ShouldReturnEmptyWhenDividingHorizontallyWithSegmentCountZero()
        {
            var sut = new Rect().DivideHorizontal(0, 0).ToArray();
            Assert.Zero(sut.Length);
        }

        [Test]
        public void ShouldReturnEmptyWhenDividingHorizontallyWithNegativeSegmentCount()
        {
            var sut = new Rect().DivideHorizontal(0, -5).ToArray();
            Assert.Zero(sut.Length);
        }

        [Test]
        public void ShouldReturnPositionWhenDividingHorizontallyWithSegmentCountOne()
        {
            var rect = new Rect(1, 2, 3, 4);
            var sut = rect.DivideHorizontal(0, 1).ToArray();

            Assert.AreEqual(1, sut.Length);
            Assert.AreEqual(rect, sut[0]);
        }

        [Test]
        public void ShouldDivideHorizontallyInEqualSegments()
        {
            var rect = new Rect(1, 2, 9, 4);
            var sut = rect.DivideHorizontal(0, 3).ToArray();

            Assert.True(sut.All(x => Mathf.Approximately(x.width, 3)));
        }

        [Test]
        public void ShouldRetainOriginalOffsetAndHeightWhenDividingHorizontally()
        {
            var rect = new Rect(1, 2, 9, 4);
            var sut = rect.DivideHorizontal(0, 3).ToArray();

            Assert.True(Mathf.Approximately(sut[0].x, 1));
            Assert.True(sut.All(x => Mathf.Approximately(x.height, 4)));
        }

        [Test]
        public void ShouldDivideHorizontallyRespectingSegmentSize()
        {
            var rect = new Rect(0, 0, 10, 1);
            var sut = rect.DivideHorizontal(0, 2, null, 3).ToArray();

            Assert.True(Mathf.Approximately(sut[0].x, 0));
            Assert.True(Mathf.Approximately(sut[1].x, 2));
            Assert.True(Mathf.Approximately(sut[2].x, 7));
        }

        [Test]
        public void ShouldDivideHorizontallyRespectingSpacing()
        {
            var rect = new Rect(0, 0, 10, 1);
            var sut = rect.DivideHorizontal(1, 1, null, 2).ToArray();

            Assert.True(Mathf.Approximately(sut[0].x, 0));
            Assert.True(Mathf.Approximately(sut[1].x, 2));
            Assert.True(Mathf.Approximately(sut[1].width, 5));
            Assert.True(Mathf.Approximately(sut[2].x, 8));
        }

        [Test]
        public void ShouldInsetByTheGivenMargins()
        {
            var rect = new Rect(0, 1, 10, 8);
            var sut = rect.Inset(left: 2, right: 1, top: 3, bottom: 4);

            Assert.AreEqual(new Rect(2, 4, 7, 1), sut);
        }

        [Test]
        public void ShouldNotInsetByDefault()
        {
            var rect = new Rect(0, 1, 10, 8);
            var sut = rect.Inset();

            Assert.AreEqual(sut, rect);
        }
    }
}
