using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Textures;
using MoonriseGames.Toolbox.Validation;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class TranslationSpritesTest
    {
        [Test]
        public void ShouldProvideSprite()
        {
            var sprite01 = TextureFactory.GetOnePixelTexture(Color.black).ToSprite();
            var sprite02 = TextureFactory.GetOnePixelTexture(Color.white).ToSprite();

            var value01 = new TranslationValue<Sprite>(SystemLanguage.Arabic, sprite01);
            var value02 = new TranslationValue<Sprite>(SystemLanguage.French, sprite02);

            var sut = ScriptableObject.CreateInstance<TranslationSprites>();

            sut.SetNonPublicField(new[] { value01, value02 });

            Assert.AreEqual(sprite01, sut.GetSprite(SystemLanguage.Arabic));
            Assert.AreEqual(sprite02, sut.GetSprite(SystemLanguage.French));
        }

        [Test]
        public void ShouldProvideFallbackSpriteForMissingLanguage()
        {
            var sprite = TextureFactory.GetOnePixelTexture(Color.white).ToSprite();
            var fallbackSprite = TextureFactory.GetOnePixelTexture(Color.black).ToSprite();

            var value = new TranslationValue<Sprite>(SystemLanguage.Arabic, sprite);
            var sut = ScriptableObject.CreateInstance<TranslationSprites>();

            sut.SetNonPublicField("_fallbackSprite", fallbackSprite);
            sut.SetNonPublicField(new[] { value });

            Assert.AreEqual(fallbackSprite, sut.GetSprite(SystemLanguage.Spanish));
        }

        [Test]
        public void ShouldProvideFallbackSpriteForNullTranslations()
        {
            var fallbackSprite = TextureFactory.GetOnePixelTexture(Color.black).ToSprite();
            var sut = ScriptableObject.CreateInstance<TranslationSprites>();

            sut.SetNonPublicField("_fallbackSprite", fallbackSprite);
            sut.SetNonPublicField(null as TranslationValue<Sprite>[]);

            Assert.AreEqual(fallbackSprite, sut.GetSprite(SystemLanguage.Spanish));
        }

        [Test]
        public void ShouldReactToTranslationUpdates()
        {
            var sprite01 = TextureFactory.GetOnePixelTexture(Color.black).ToSprite();
            var sprite02 = TextureFactory.GetOnePixelTexture(Color.white).ToSprite();

            var value01 = new TranslationValue<Sprite>(SystemLanguage.French, sprite01);
            var value02 = new TranslationValue<Sprite>(SystemLanguage.French, sprite02);

            var sut = ScriptableObject.CreateInstance<TranslationSprites>();

            sut.SetNonPublicField(new[] { value01 });
            Assert.AreEqual(sprite01, sut.GetSprite(SystemLanguage.French));

            sut.SetNonPublicField(new[] { value02 });
            Assert.AreEqual(sprite02, sut.GetSprite(SystemLanguage.French));
        }

        [Test]
        public void ShouldBeInvalidWithDuplicateLanguages()
        {
            var value01 = new TranslationValue<Sprite>(SystemLanguage.Arabic, null);
            var value02 = new TranslationValue<Sprite>(SystemLanguage.Arabic, null);

            var sut = ScriptableObject.CreateInstance<TranslationSprites>();

            sut.SetNonPublicField(new[] { value01, value02 });

            Assert.Throws<ValidationException>(() => sut.Validate());
        }
    }
}
