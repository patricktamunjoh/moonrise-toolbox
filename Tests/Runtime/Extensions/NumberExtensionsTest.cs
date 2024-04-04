using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class NumberExtensionsTest
    {
        [Test]
        public void ShouldDetectSimilarValues()
        {
            Assert.True(0f.IsSimilar(0f));
            Assert.True(0.3f.IsSimilar(0.1f + 0.1f + 0.1f));
        }

        [Test]
        public void ShouldNotDetectNonSimilarValues()
        {
            Assert.False(0f.IsSimilar(1f));
            Assert.False(0f.IsSimilar(0.000000001f));
        }

        [Test]
        public void ShouldConvertToRomanNumber()
        {
            Assert.AreEqual("I", 1.ToRoman());
            Assert.AreEqual("III", 3.ToRoman());
            Assert.AreEqual("IV", 4.ToRoman());
            Assert.AreEqual("V", 5.ToRoman());
            Assert.AreEqual("VI", 6.ToRoman());
            Assert.AreEqual("VII", 7.ToRoman());
            Assert.AreEqual("IX", 9.ToRoman());
            Assert.AreEqual("X", 10.ToRoman());
            Assert.AreEqual("XI", 11.ToRoman());
            Assert.AreEqual("XIII", 13.ToRoman());
            Assert.AreEqual("XIV", 14.ToRoman());
            Assert.AreEqual("XV", 15.ToRoman());
            Assert.AreEqual("XVI", 16.ToRoman());
            Assert.AreEqual("XIX", 19.ToRoman());
            Assert.AreEqual("XX", 20.ToRoman());
            Assert.AreEqual("XXI", 21.ToRoman());
            Assert.AreEqual("XXIII", 23.ToRoman());
            Assert.AreEqual("XXIV", 24.ToRoman());
            Assert.AreEqual("XXV", 25.ToRoman());
            Assert.AreEqual("XXVI", 26.ToRoman());
            Assert.AreEqual("XXIX", 29.ToRoman());
            Assert.AreEqual("XXXIV", 34.ToRoman());
            Assert.AreEqual("XXXV", 35.ToRoman());
            Assert.AreEqual("XXXVI", 36.ToRoman());
            Assert.AreEqual("XXXIX", 39.ToRoman());
        }

        [Test]
        public void ShouldNormalizeAngle()
        {
            Assert.True(0f.IsSimilar(360f.NormalizeAngle()));
            Assert.True((-10f).IsSimilar(350f.NormalizeAngle()));
            Assert.True((-60f).IsSimilar(660f.NormalizeAngle()));
            Assert.True(180f.IsSimilar(180f.NormalizeAngle()));
            Assert.True(180f.IsSimilar((-180f).NormalizeAngle()));
            Assert.True(180f.IsSimilar(540f.NormalizeAngle()));
            Assert.True(170f.IsSimilar((-190f).NormalizeAngle()));
            Assert.True((-100f).IsSimilar((-100f).NormalizeAngle()));
        }
    }
}
