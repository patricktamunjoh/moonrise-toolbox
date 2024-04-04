using System.Collections;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Tests.Utilities.Factories;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class LocalizationServiceTest
    {
        [SetUp]
        public void Setup() => Function.ClearScene();

        [Test]
        public void ShouldProvideTranslation()
        {
            var translation = LocalizationFactory.GetTranslation("key,english,german", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.English);

            Assert.AreEqual("a", sut.GetLocalization("key"));
        }

        [Test]
        public void ShouldProvideFallbackForMissingTranslation()
        {
            var sut = new GameObject().AddComponent<LocalizationService>();

            Assert.AreEqual("n/a", sut.GetLocalization("key"));
            Assert.AreEqual("fallback", sut.GetLocalization("key", "fallback"));
            Assert.Null(sut.GetLocalization("key", null));
        }

        [Test]
        public void ShouldProvideClosestKey()
        {
            var translation = LocalizationFactory.GetTranslation("key,english,german", "keyone,a,b", "keytwo,c,d");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);

            Assert.AreEqual("keytwo", sut.GetClosestKey("two"));
        }

        [Test]
        public void ShouldProvideFallbackForMissingClosestKey()
        {
            var sut = new GameObject().AddComponent<LocalizationService>();

            Assert.AreEqual(string.Empty, sut.GetClosestKey("key", string.Empty));
            Assert.AreEqual("fallback", sut.GetClosestKey("key", "fallback"));
            Assert.Null(sut.GetClosestKey("key"));
        }

        [Test]
        public void ShouldProvideLocalizationAvailability()
        {
            var translation = LocalizationFactory.GetTranslation("key,english,german", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            Assert.False(sut.IsLocalizationAvailable("key"));

            sut.SetNonPublicField(translation);
            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.English);

            Assert.True(sut.IsLocalizationAvailable("key"));
        }

        [Test]
        public void ShouldProvideAvailableLanguages()
        {
            var translation = LocalizationFactory.GetTranslation("key,english,german", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.English);

            Assert.True(sut.AvailableLanguages.SequenceEqual(new[] { SystemLanguage.English, SystemLanguage.German }));
        }

        [Test]
        public void ShouldUseEditorLanguageInEditorMode()
        {
            var translation = LocalizationFactory.GetTranslation("key,english,german", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.English);

            Assert.AreEqual("a", sut.GetLocalization("key"));

            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.German);

            Assert.AreEqual("b", sut.GetLocalization("key"));
        }

        [UnityTest]
        public IEnumerator ShouldReloadLanguageFilesWhenChanged()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var translation = ScriptableObject.CreateInstance<Translation>();
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            translation.SetNonPublicField(new[] { file });

            yield return null;

            Assert.AreEqual("a", sut.GetLocalization("key"));
        }

        [UnityTest]
        public IEnumerator ShouldRefreshLocalizationAfterLanguageFileReload()
        {
            var file = new TextAsset("key,english,german\nkey,a,b");
            var translation = ScriptableObject.CreateInstance<Translation>();
            var localizeable = new GameObject().AddComponent<SampleLocalizable>();
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            translation.SetNonPublicField(new[] { file });

            Assert.AreEqual(0, localizeable.LocalizeCount);
            yield return null;

            Assert.AreEqual(2, localizeable.LocalizeCount);
        }

        [UnityTest]
        public IEnumerator ShouldRefreshLocalizationAfterEditorLanguageChange()
        {
            var localizeable = new GameObject().AddComponent<SampleLocalizable>();
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.Italian);
            yield return null;

            Assert.AreEqual(1, localizeable.LocalizeCount);
        }

        [Test]
        public void ShouldRefreshLocalizationWhenSettingLanguage()
        {
            var localizeable = new GameObject().AddComponent<SampleLocalizable>();
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.Italian);
            sut.SetLanguage(SystemLanguage.Basque);

            Assert.AreEqual(1, localizeable.LocalizeCount);
        }

        [Test]
        public void ShouldRefreshLocalizationWhenClearingLanguage()
        {
            var localizeable = new GameObject().AddComponent<SampleLocalizable>();
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField("_editorModeLanguage", SystemLanguage.Italian);
            sut.ClearLanguage();

            Assert.AreEqual(1, localizeable.LocalizeCount);
        }

        private class SampleLocalizable : MonoBehaviour, ILocalizable
        {
            public int LocalizeCount { get; private set; }

            public void Localize() => LocalizeCount++;
        }
    }
}
