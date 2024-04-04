using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Validation;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class TranslationTest
    {
        [Test]
        public void ShouldReadLanguageFiles()
        {
            var file01 = new TextAsset("key,english,german\nkey01,a,b");
            var file02 = new TextAsset("key,english,german\nkey02,c,d");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file01, file02 });
            sut.RefreshLanguageData();

            Assert.AreEqual("a", sut[("key01", SystemLanguage.English)]);
            Assert.AreEqual("c", sut[("key02", SystemLanguage.English)]);
        }

        [Test]
        public void ShouldProvideTranslation()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.RefreshLanguageData();

            Assert.AreEqual("a", sut[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", sut[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldProvideTranslationUsingFallbackLanguage()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.SetNonPublicField("_fallbackLanguage", SystemLanguage.English);
            sut.RefreshLanguageData();

            Assert.AreEqual("a", sut[("key", SystemLanguage.Spanish)]);
        }

        [Test]
        public void ShouldProvideNullForMissingTranslationMap()
        {
            var sut = ScriptableObject.CreateInstance<Translation>();

            Assert.Null(sut[("key", SystemLanguage.English)]);
        }

        [Test]
        public void ShouldProvideNullForInvalidKey()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.RefreshLanguageData();

            Assert.Null(sut[("", SystemLanguage.English)]);
            Assert.Null(sut[(null, SystemLanguage.English)]);
            Assert.Null(sut[("invalid", SystemLanguage.English)]);
        }

        [Test]
        public void ShouldProvideNullForInvalidLanguage()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.SetNonPublicField("_fallbackLanguage", SystemLanguage.Japanese);
            sut.RefreshLanguageData();

            Assert.Null(sut[("key", SystemLanguage.Spanish)]);
            Assert.Null(sut[("key", SystemLanguage.Afrikaans)]);
        }

        [Test]
        public void ShouldProvideClosestKey()
        {
            var file = new TextAsset("key,english,german\nkey one,a,b\nkey two,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.RefreshLanguageData();

            Assert.AreEqual("key two", sut.GetClosestKey("two"));
        }

        [Test]
        public void ShouldProvideNullAsClosestKeyForMissingTranslationMap()
        {
            var sut = ScriptableObject.CreateInstance<Translation>();

            Assert.Null(sut.GetClosestKey("two"));
        }

        [Test]
        public void ShouldCheckIfLanguageFilesChanged()
        {
            var file01 = new TextAsset("key,english,german\nkey,a,b");
            var file02 = new TextAsset("key,english,german\nkey,c,d");
            var sut = ScriptableObject.CreateInstance<Translation>();

            Assert.False(sut.RequiresRefresh());

            sut.SetNonPublicField(new[] { file01 });
            Assert.True(sut.RequiresRefresh());

            sut.RefreshLanguageData();

            Assert.False(sut.RequiresRefresh());

            sut.SetNonPublicField(new[] { file02 });
            Assert.True(sut.RequiresRefresh());
        }

        [Test]
        public void ShouldBeInvalidWithMissingLanguageFile()
        {
            var sut = ScriptableObject.CreateInstance<Translation>();

            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeInvalidWithUnavailableFallbackLanguage()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.SetNonPublicField("_fallbackLanguage", SystemLanguage.Spanish);
            sut.RefreshLanguageData();

            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeValidWithCorrectSetup()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var sut = ScriptableObject.CreateInstance<Translation>();

            sut.SetNonPublicField(new[] { file });
            sut.SetNonPublicField("_fallbackLanguage", SystemLanguage.English);
            sut.RefreshLanguageData();

            sut.Validate();
        }
    }
}
