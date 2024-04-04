using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Paths;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Paths
{
    public class BezierPathTest
    {
        [Test]
        public void ShouldCreateTheSetNumberOfPoints()
        {
            var sut = new BezierPath(new[] { Vector3.zero, Vector3.one }, 5);
            Assert.AreEqual(5, sut.PointCount);
        }

        [Test]
        public void ShouldHandleOneControlPoint()
        {
            var sut = new BezierPath(new[] { Vector3.one }, 5);

            Assert.True(Vector3.one.IsSimilar(sut[0]));
            Assert.True(Vector3.one.IsSimilar(sut[4]));
        }

        [Test]
        public void ShouldHandleTwoControlPoints()
        {
            var sut = new BezierPath(new[] { Vector3.one, Vector3.forward }, 5);

            Assert.True(Vector3.one.IsSimilar(sut[0]));
            Assert.True(Vector3.forward.IsSimilar(sut[4]));
        }

        [Test]
        public void ShouldHandleThreeControlPoints()
        {
            var sut = new BezierPath(new[] { Vector3.one, Vector3.zero, Vector3.forward }, 5);

            Assert.True(Vector3.one.IsSimilar(sut[0]));
            Assert.True(Vector3.forward.IsSimilar(sut[4]));
        }

        [Test]
        public void ShouldHandleFourControlPoints()
        {
            var sut = new BezierPath(new[] { Vector3.one, Vector3.left, Vector3.right, Vector3.forward }, 5);

            Assert.True(Vector3.one.IsSimilar(sut[0]));
            Assert.True(Vector3.forward.IsSimilar(sut[4]));
        }
    }
}
