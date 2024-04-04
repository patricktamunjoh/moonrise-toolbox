using System;
using System.Linq;
using MoonriseGames.Toolbox.Localization;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class TranslationMapTest
    {
        [Test]
        public void ShouldProvideTranslation()
        {
            var set01 = new TranslationSet<string>(
                "key01",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var set02 = new TranslationSet<string>(
                "key02",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "hello"),
                    new TranslationValue<string>(SystemLanguage.German, "hallo")
                }
            );

            var sut = new TranslationMap<string>(new[] { set01, set02 });

            Assert.AreEqual("example", sut[("key01", SystemLanguage.English)]);
            Assert.AreEqual("beispiel", sut[("key01", SystemLanguage.German)]);
            Assert.AreEqual("hello", sut[("key02", SystemLanguage.English)]);
            Assert.AreEqual("hallo", sut[("key02", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldProvideTranslationByLowercaseKey()
        {
            var set = new TranslationSet<string>("key", new[] { new TranslationValue<string>(SystemLanguage.English, "example") });
            var sut = new TranslationMap<string>(new[] { set });

            Assert.AreEqual("example", sut[("KEY", SystemLanguage.English)]);
        }

        [Test]
        public void ShouldProvideLanguages()
        {
            var set = new TranslationSet<string>(
                "key01",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var sut = new TranslationMap<string>(new[] { set });

            Assert.True(sut.Languages.SequenceEqual(new[] { SystemLanguage.English, SystemLanguage.German }));
        }

        [Test]
        public void ShouldCheckIfLanguageIsContained()
        {
            var set = new TranslationSet<string>(
                "key01",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var sut = new TranslationMap<string>(new[] { set });

            Assert.True(sut.ContainsLanguage(SystemLanguage.English));
            Assert.True(sut.ContainsLanguage(SystemLanguage.German));
            Assert.False(sut.ContainsLanguage(SystemLanguage.Spanish));
        }

        [Test]
        public void ShouldProvideCount()
        {
            var set01 = new TranslationSet<string>(
                "key01",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var set02 = new TranslationSet<string>(
                "key02",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "hello"),
                    new TranslationValue<string>(SystemLanguage.German, "hallo")
                }
            );

            var sut = new TranslationMap<string>(new[] { set01, set02 });

            Assert.AreEqual(2, sut.Count);
        }

        [Test]
        public void ShouldCheckIfKeyIsContained()
        {
            var set01 = new TranslationSet<string>(
                "key01",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var set02 = new TranslationSet<string>(
                "key02",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "hello"),
                    new TranslationValue<string>(SystemLanguage.German, "hallo")
                }
            );

            var sut = new TranslationMap<string>(new[] { set01, set02 });

            Assert.True(sut.ContainsKey("key01"));
            Assert.True(sut.ContainsKey("key02"));

            Assert.False(sut.ContainsKey("key03"));
            Assert.False(sut.ContainsKey("key"));
        }

        [Test]
        public void ShouldCheckIfKeyIsContainedIgnoringCase()
        {
            var set = new TranslationSet<string>(
                "key",
                new[]
                {
                    new TranslationValue<string>(SystemLanguage.English, "example"),
                    new TranslationValue<string>(SystemLanguage.German, "beispiel")
                }
            );

            var sut = new TranslationMap<string>(new[] { set });

            Assert.True(sut.ContainsKey("KEY"));
        }

        [Test]
        public void ShouldProvideShortestClosestKey()
        {
            var set01 = new TranslationSet<string>(
                "very long key",
                new[] { new TranslationValue<string>(SystemLanguage.English, "example") }
            );
            var set02 = new TranslationSet<string>("short key", new[] { new TranslationValue<string>(SystemLanguage.English, "hello") });

            var sut = new TranslationMap<string>(new[] { set01, set02 });

            Assert.AreEqual("short key", sut.GetClosestKey("key"));
        }

        [Test]
        public void ShouldProvideKeyMatchingQueryElements()
        {
            var set = new TranslationSet<string>(
                "example_local-key demo",
                new[] { new TranslationValue<string>(SystemLanguage.English, "example") }
            );
            var sut = new TranslationMap<string>(new[] { set });

            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("key"));
            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("exa"));
            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("demo-loc-ample"));
            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("example local key demo"));
            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("example-local_key:demo"));
            Assert.AreEqual("example_local-key demo", sut.GetClosestKey("example:local"));

            Assert.Null(sut.GetClosestKey("invalid"));
            Assert.Null(sut.GetClosestKey("example invalid"));
        }

        [Test]
        public void ShouldProvideKeyIgnoringCase()
        {
            var set = new TranslationSet<string>("example key", new[] { new TranslationValue<string>(SystemLanguage.English, "example") });
            var sut = new TranslationMap<string>(new[] { set });

            Assert.AreEqual("example key", sut.GetClosestKey("KEY"));
            Assert.AreEqual("example key", sut.GetClosestKey("EXAMPLE"));
        }

        [Test]
        public void ShouldThrowIfSetsNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new TranslationMap<string>(null));
            Assert.Throws<ArgumentException>(() => new TranslationMap<string>(Array.Empty<TranslationSet<string>>()));
        }

        [Test]
        public void ShouldThrowIfSetsLanguageMismatch()
        {
            var set01 = new TranslationSet<string>("key01", new[] { new TranslationValue<string>(SystemLanguage.English, "example") });
            var set02 = new TranslationSet<string>("key02", new[] { new TranslationValue<string>(SystemLanguage.Spanish, "ejemplo") });

            Assert.Throws<ArgumentException>(() => new TranslationMap<string>(new[] { set01, set02 }));
        }
    }
}
