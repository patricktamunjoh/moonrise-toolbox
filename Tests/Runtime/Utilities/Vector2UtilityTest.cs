using MoonriseGames.Toolbox.Utilities;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Utilities
{
    public class Vector2UtilityTest
    {
        [Test]
        public void ShouldDetectIntersectionInLines()
        {
            Assert.AreEqual(
                new Vector2(0.5f, 0.5f),
                Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1))
            );

            Assert.AreEqual(
                new Vector2(0.5f, 0.5f),
                Vector2Utility.GetIntersection(new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1))
            );

            Assert.AreEqual(
                new Vector2(0.5f, 0.5f),
                Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0))
            );
        }

        [Test]
        public void ShouldDetectIntersectionInLinesOnEndPoint()
        {
            Assert.AreEqual(
                new Vector2(0f, 0f),
                Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(0, 1))
            );

            Assert.AreEqual(
                new Vector2(1f, 1f),
                Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, 2))
            );
        }

        [Test]
        public void ShouldNotDetectIntersectionInParallelLines()
        {
            Assert.Null(Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1)));
            Assert.Null(Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 1), new Vector2(2, 2)));
            Assert.Null(Vector2Utility.GetIntersection(new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 1)));
        }

        [Test]
        public void ShouldNotDetectIntersectionsOutsideLines()
        {
            Assert.Null(Vector2Utility.GetIntersection(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2), new Vector2(0, 2)));
            Assert.Null(Vector2Utility.GetIntersection(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 0), new Vector2(0, 8)));
        }
    }
}
