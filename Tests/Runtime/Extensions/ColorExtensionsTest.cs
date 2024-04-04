using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class ColorExtensionsTest
    {
        [Test]
        public void ShouldUpdateProperties()
        {
            Assert.AreEqual(1, Color.black.WithR(1).r);
            Assert.AreEqual(1, Color.black.WithG(1).g);
            Assert.AreEqual(1, Color.black.WithB(1).b);

            Assert.Zero(Color.black.WithAlpha(0).a);
        }

        [Test]
        public void ShouldDetectSimilarColors()
        {
            Assert.True(new Color(1, 1, 1, 1).IsSimilar(Color.white));
            Assert.True(new Color(0.3f, 0.3f, 0.3f, 0.3f).IsSimilar(new Color(0.1f + 0.2f, 0.3f, 0.6f / 2, 0.3f)));
        }

        [Test]
        public void ShouldNotDetectNonSimilarColors()
        {
            Assert.False(Color.black.IsSimilar(Color.white));
            Assert.False(new Color(0.3f, 0.3f, 0.3f, 0.3f).IsSimilar(new Color(0.31f, 0.3f, 0.3f, 0.3f)));
        }
    }
}
