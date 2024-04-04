using System;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Paths;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Paths
{
    public class PathTest
    {
        [Test]
        public void ShouldProvidePointCount()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.forward, Vector3.left, });
            Assert.AreEqual(3, sut.PointCount);
        }

        [Test]
        public void ShouldProvideTotalLength()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.zero, Vector3.left, });
            Assert.AreEqual(2, sut.Length);
        }

        [Test]
        public void ShouldThrowForInsufficientPoints()
        {
            Assert.Throws<ArgumentException>(() => new Path(null));
            Assert.Throws<ArgumentException>(() => new Path(new[] { Vector3.zero, }));
        }

        [Test]
        public void ShouldRetainPointPlacements()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.forward, Vector3.left, });

            Assert.AreEqual(Vector3.down, sut[0]);
            Assert.AreEqual(Vector3.forward, sut[1]);
            Assert.AreEqual(Vector3.left, sut[2]);
        }

        [Test]
        public void ShouldSampleRelativePoints()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.up, });

            Assert.True(new Vector3(0, 0, 0).IsSimilar(sut.Sample(0.5f)));
            Assert.True(new Vector3(0, 0.5f, 0).IsSimilar(sut.Sample(0.75f)));
            Assert.True(new Vector3(0, -0.5f, 0).IsSimilar(sut.Sample(0.25f)));
        }

        [Test]
        public void ShouldSampleStartForNegativeProgress()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.up, });

            Assert.True(Vector3.down.IsSimilar(sut.Sample(-1)));
            Assert.True(Vector3.down.IsSimilar(sut.Sample(0)));
        }

        [Test]
        public void ShouldSampleEndForProgressBeyondOne()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.up, });

            Assert.True(Vector3.up.IsSimilar(sut.Sample(1)));
            Assert.True(Vector3.up.IsSimilar(sut.Sample(2)));
        }

        [Test]
        public void ShouldProvideSampleForFloatIndex()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.up, });

            Assert.True(sut.Sample(0.125f).IsSimilar(sut[0.125f]));
            Assert.True(sut.Sample(0.668f).IsSimilar(sut[0.668f]));
            Assert.True(sut.Sample(0.307f).IsSimilar(sut[0.307f]));
        }

        [Test]
        public void ShouldRemoveRedundantPoints()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.down * .5f, Vector3.zero, Vector3.right }).Optimize(10);
            Assert.AreEqual(2, sut.PointCount);
        }

        [Test]
        public void ShouldNotRemoveStartAndEndPoint()
        {
            var sut = new Path(new[] { Vector3.down, Vector3.zero, Vector3.right }).Optimize(100);

            Assert.AreEqual(Vector3.down, sut[0]);
            Assert.AreEqual(Vector3.right, sut[1]);
        }
    }
}
