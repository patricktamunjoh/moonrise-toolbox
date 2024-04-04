using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Tests.Utilities.Factories;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Playmode.Tests.Localization
{
    public class LocalizationServiceTest
    {
        [SetUp]
        public void Setup() => Function.ClearScene();

        [Test]
        public void ShouldUseStoredLanguageInPlayMode()
        {
            var translation = LocalizationFactory.GetTranslation("key,spanish,italian", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            sut.SetLanguage(SystemLanguage.Spanish);

            Assert.AreEqual("a", sut.GetLocalization("key"));

            sut.SetLanguage(SystemLanguage.Italian);
            Assert.AreEqual("b", sut.GetLocalization("key"));
        }

        [Test]
        public void ShouldUseSystemLanguageAsFallbackInPlayMode()
        {
            var translation = LocalizationFactory.GetTranslation($"key,spanish,{Application.systemLanguage}", "key,a,b");
            var sut = new GameObject().AddComponent<LocalizationService>();

            sut.SetNonPublicField(translation);
            sut.ClearLanguage();

            Assert.AreEqual("b", sut.GetLocalization("key"));
        }
    }
}
