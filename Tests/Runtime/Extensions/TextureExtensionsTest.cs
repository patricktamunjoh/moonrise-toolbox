using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Textures;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class TextureExtensionsTest
    {
        [Test]
        public void ShouldCreateSpriteFromTexture()
        {
            var texture = TextureFactory.GetCheckerboardTexture(5, Color.black, Color.white);
            var sut = texture.ToSprite();

            Assert.AreEqual(new Rect(0, 0, texture.width, texture.height), sut.rect);
            Assert.AreEqual(new Vector2(2.5f, 2.5f), sut.pivot);
            Assert.AreEqual(texture, sut.texture);
        }
    }
}
