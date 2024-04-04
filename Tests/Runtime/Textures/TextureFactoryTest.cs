using MoonriseGames.Toolbox.Textures;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Textures
{
    public class TextureFactoryTest
    {
        [Test]
        public void ShouldCreateCheckerboardTexture()
        {
            var sut = TextureFactory.GetCheckerboardTexture(10, Color.black, Color.white);

            Assert.AreNotEqual(sut.GetPixel(0, 0), sut.GetPixel(0, 1));
            Assert.AreEqual(10, sut.width);
            Assert.AreEqual(10, sut.height);
            Assert.AreEqual(FilterMode.Point, sut.filterMode);
        }

        [Test]
        public void ShouldCreateOnePixelTexture()
        {
            var sut = TextureFactory.GetOnePixelTexture(Color.black);

            Assert.AreEqual(Color.black, sut.GetPixel(0, 0));
            Assert.AreEqual(1, sut.width);
            Assert.AreEqual(1, sut.height);
        }
    }
}
