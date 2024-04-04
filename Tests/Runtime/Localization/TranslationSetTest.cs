using System;
using System.Linq;
using MoonriseGames.Toolbox.Localization;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class TranslationSetTest
    {
        [Test]
        public void ShouldProvideValueForLanguage()
        {
            var sut = new TranslationSet<string>(
                "key",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            Assert.AreEqual("example", sut[SystemLanguage.English]);
            Assert.AreEqual("beispiel", sut[SystemLanguage.German]);
        }

        [Test]
        public void ShouldProvideLanguages()
        {
            var sut = new TranslationSet<string>(
                "key",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            Assert.True(sut.Languages.SequenceEqual(new[] { SystemLanguage.English, SystemLanguage.German }));
        }

        [Test]
        public void ShouldTransformKeyToLowercase()
        {
            var sut = new TranslationSet<string>("KEY", new[] { new TranslationValue<string>(SystemLanguage.English, "example") });

            Assert.AreEqual("key", sut.Key);
        }

        [Test]
        public void ShouldThrowIfValuesNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new TranslationSet<string>("key", null));
            Assert.Throws<ArgumentException>(() => new TranslationSet<string>("key", Array.Empty<TranslationValue<string>>()));
        }

        [Test]
        public void ShouldThrowKeyNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(
                () => new TranslationSet<string>("", new[] { new TranslationValue<string>(SystemLanguage.Spanish, "ejemplo") })
            );
            Assert.Throws<ArgumentException>(
                () => new TranslationSet<string>(null, new[] { new TranslationValue<string>(SystemLanguage.Spanish, "ejemplo") })
            );
        }
    }
}
