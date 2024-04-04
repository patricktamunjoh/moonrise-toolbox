using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class VectorExtensionsTest
    {
        [Test]
        public void ShouldMoveTowardsTargetVector3()
        {
            var sut = Vector3.zero;
            Vector3.one.IsSimilar(sut.MoveTowards(Vector3.one, 100));
        }

        [Test]
        public void ShouldMoveTowardsTargetVector2()
        {
            var sut = Vector2.zero;
            Vector2.one.IsSimilar(sut.MoveTowards(Vector2.one, 100));
        }

        [Test]
        public void ShouldNotExceedMaxDistanceVector3()
        {
            var sut = Vector3.zero;
            Vector3.one.IsSimilar(sut.MoveTowards(Vector3.one * 100, 1));
        }

        [Test]
        public void ShouldNotExceedMaxDistanceVector2()
        {
            var sut = Vector2.zero;
            Vector2.one.IsSimilar(sut.MoveTowards(Vector2.one * 100, 1));
        }

        [Test]
        public void ShouldSumVector4()
        {
            var sut = new Vector4(1, 2, 3, 4);
            Assert.AreEqual(10, sut.Sum());
        }

        [Test]
        public void ShouldSumVector3()
        {
            var sut = new Vector3(1, 2, 3);
            Assert.AreEqual(6, sut.Sum());
        }

        [Test]
        public void ShouldSumVector2()
        {
            var sut = new Vector2(1, 2);
            Assert.AreEqual(3, sut.Sum());
        }

        [Test]
        public void ShouldUpdatePropertiesVector4()
        {
            Assert.AreEqual(5, Vector4.zero.WithX(5).x);
            Assert.AreEqual(5, Vector4.zero.WithY(5).y);
            Assert.AreEqual(5, Vector4.zero.WithZ(5).z);
            Assert.AreEqual(5, Vector4.zero.WithW(5).w);
        }

        [Test]
        public void ShouldUpdatePropertiesVector3()
        {
            Assert.AreEqual(5, Vector3.zero.WithX(5).x);
            Assert.AreEqual(5, Vector3.zero.WithY(5).y);
            Assert.AreEqual(5, Vector3.zero.WithZ(5).z);
        }

        [Test]
        public void ShouldUpdatePropertiesVector2()
        {
            Assert.AreEqual(5, Vector2.zero.WithX(5).x);
            Assert.AreEqual(5, Vector2.zero.WithY(5).y);
        }

        [Test]
        public void ShouldAddPropertiesVector4()
        {
            Assert.AreEqual(6, Vector4.one.AddX(5).x);
            Assert.AreEqual(6, Vector4.one.AddY(5).y);
            Assert.AreEqual(6, Vector4.one.AddZ(5).z);
            Assert.AreEqual(6, Vector4.one.AddW(5).w);
        }

        [Test]
        public void ShouldAddPropertiesVector3()
        {
            Assert.AreEqual(6, Vector3.one.AddX(5).x);
            Assert.AreEqual(6, Vector3.one.AddY(5).y);
            Assert.AreEqual(6, Vector3.one.AddZ(5).z);
        }

        [Test]
        public void ShouldAddPropertiesVector2()
        {
            Assert.AreEqual(6, Vector2.one.AddX(5).x);
            Assert.AreEqual(6, Vector2.one.AddY(5).y);
        }

        [Test]
        public void ShouldDetectSimilarVector4()
        {
            Assert.True(new Vector4(0.3f, 0.3f, 0.3f, 0.3f).IsSimilar(Vector4.one * 0.1f + Vector4.one * 0.2f));
            Assert.True(new Vector4(0.3f, 0.3f, 0.3f, 0.3f).IsSimilar(new Vector4(0.1f + 0.2f, 0.3f, 0.6f / 2, 0.3f)));
        }

        [Test]
        public void ShouldNotDetectNonSimilarVector4()
        {
            Assert.False(Vector4.one.IsSimilar(Vector4.zero));
            Assert.False(new Vector4(0.3f, 0.3f, 0.3f, 0.3f).IsSimilar(new Vector4(0.31f, 0.3f, 0.3f, 0.3f)));
        }

        [Test]
        public void ShouldDetectSimilarVector3()
        {
            Assert.True(new Vector3(0.3f, 0.3f, 0.3f).IsSimilar(Vector3.one * 0.1f + Vector3.one * 0.2f));
            Assert.True(new Vector3(0.3f, 0.3f, 0.3f).IsSimilar(new Vector3(0.1f + 0.2f, 0.3f, 0.6f / 2)));
        }

        [Test]
        public void ShouldNotDetectNonSimilarVector3()
        {
            Assert.False(Vector3.one.IsSimilar(Vector3.zero));
            Assert.False(new Vector3(0.3f, 0.3f, 0.3f).IsSimilar(new Vector3(0.31f, 0.3f, 0.3f)));
        }

        [Test]
        public void ShouldDetectSimilarVector2()
        {
            Assert.True(new Vector2(0.3f, 0.3f).IsSimilar(Vector2.one * 0.1f + Vector2.one * 0.2f));
            Assert.True(new Vector2(0.3f, 0.3f).IsSimilar(new Vector2(0.1f + 0.2f, 0.6f / 2)));
        }

        [Test]
        public void ShouldNotDetectNonSimilarVector2()
        {
            Assert.False(Vector2.one.IsSimilar(Vector2.zero));
            Assert.False(new Vector2(0.3f, 0.3f).IsSimilar(new Vector2(0.31f, 0.3f)));
        }

        [Test]
        public void ShouldFindPerpendicularDistanceToLine()
        {
            Assert.True(Vector3.right.DistanceToLine(Vector3.zero, Vector3.forward).IsSimilar(1));
            Assert.True(Vector3.zero.DistanceToLine(Vector3.zero, Vector3.forward).IsSimilar(0));
        }
    }
}
