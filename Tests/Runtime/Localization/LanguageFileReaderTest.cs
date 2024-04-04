using System;
using System.Linq;
using MoonriseGames.Toolbox.Localization;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class LanguageFileReaderTest
    {
        [Test]
        public void ShouldReadSingleFile()
        {
            var file = new TextAsset("key,english,german\nkey,example,beispiel");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.True(map.Languages.SequenceEqual(new[] { SystemLanguage.English, SystemLanguage.German }));
            Assert.True(map.ContainsKey("key"));

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("example", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("beispiel", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldReadSpecialCharacters()
        {
            var file = new TextAsset("key,english,german\nkey,äöüß,🍕🍦🍓");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual("äöüß", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("🍕🍦🍓", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldReadMultipleFiles()
        {
            var file01 = new TextAsset("key,english,german\nkey01,example01,beispiel01");
            var file02 = new TextAsset("key,english,german\nkey02,example02,beispiel02");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file01, file02 });

            Assert.True(map.Languages.SequenceEqual(new[] { SystemLanguage.English, SystemLanguage.German }));

            Assert.True(map.ContainsKey("key01"));
            Assert.True(map.ContainsKey("key02"));

            Assert.AreEqual(2, map.Count);
            Assert.AreEqual("example01", map[("key01", SystemLanguage.English)]);
            Assert.AreEqual("beispiel01", map[("key01", SystemLanguage.German)]);
            Assert.AreEqual("example02", map[("key02", SystemLanguage.English)]);
            Assert.AreEqual("beispiel02", map[("key02", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldIgnoreCommentedLines()
        {
            var file = new TextAsset("#this is a comment\nkey,english,german\n# comment\nkey,#a,#b\n # comment spaced");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("#a", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("#b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldIgnoreEmptyLines()
        {
            var file = new TextAsset("\n\n\nkey,english,german\n \n   \t \nkey,a,b\n  ");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("a", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldTrimLineStartAndEnd()
        {
            var file = new TextAsset("key,english,german  \n   \tkey,a,b  ");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("a", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldTrimElementStartAndEnd()
        {
            var file = new TextAsset("  key , english ,  german\t\n key\t,  a ,  b  ");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("a", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldCorrectlyHandleEscaping()
        {
            var file = new TextAsset("key,english,german\n\"k,ey\",\"\"\"a\"\"\",\"b \"");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("\"a\"", map[("k,ey", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("k,ey", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldAcceptDifferentOperatorSymbols()
        {
            var file = new TextAsset("~comment;line\nkey;english;german\nkey;+a;+++;b");
            var sut = new LanguageFileReader(';', '+', '~');
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("a;+", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldTolerateDelimiterAtEnd()
        {
            var file = new TextAsset("key,english,german\nkey,a,b,");
            var sut = new LanguageFileReader();
            var map = sut.ReadTranslationMap(new[] { file });

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual("a", map[("key", SystemLanguage.English)]);
            Assert.AreEqual("b", map[("key", SystemLanguage.German)]);
        }

        [Test]
        public void ShouldThrowForDuplicateKeysWithSingleFile()
        {
            var file = new TextAsset("key,english,german\nkey,a,b\nkey,c,d");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }

        [Test]
        public void ShouldThrowForDuplicateKeysWithMultipleFiles()
        {
            var file01 = new TextAsset("key,english,german\nkey,a,b");
            var file02 = new TextAsset("key,english,german\nkey,c,d");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file01, file02 }));
        }

        [Test]
        public void ShouldThrowForFileHeaderLanguageMismatch()
        {
            var file01 = new TextAsset("key,english,german\nkey01,a,b");
            var file02 = new TextAsset("key,english,spanish\nkey02,c,d");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file01, file02 }));
        }

        [Test]
        public void ShouldThrowForFileHeaderLengthMismatch()
        {
            var file01 = new TextAsset("key,english,german\nkey01,a,b");
            var file02 = new TextAsset("key,english\nkey02,c");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file01, file02 }));
        }

        [Test]
        public void ShouldThrowIfAnyFileIsEmpty()
        {
            var file01 = new TextAsset("key,english,german\nkey,a,b");
            var file02 = new TextAsset("");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file01, file02 }));
        }

        [Test]
        public void ShouldThrowIfAnyFileHasNoContentRow()
        {
            var file01 = new TextAsset("key,english,german\nkey,a,b");
            var file02 = new TextAsset("# comment row\n    \n# comment row");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file01, file02 }));
        }

        [Test]
        public void ShouldThrowIfHeaderContainsCommentSymbol()
        {
            var file = new TextAsset("k#ey,english,german\nkey,a,b");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }

        [Test]
        public void ShouldThrowIfHeaderContainsEscapeSymbol()
        {
            var file = new TextAsset("\"key\",english,german\nkey,a,b");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }

        [Test]
        public void ShouldThrowIfHeaderContainsUnknownLanguage()
        {
            var file = new TextAsset("k#ey,english,sholowinish\nkey,a,b");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }

        [Test]
        public void ShouldThrowIfRowContentDoesntMatchHeader()
        {
            var file = new TextAsset("k#ey,english,german\nkey,a,b,c");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }

        [Test]
        public void ShouldThrowIfEscapeIsNotClosed()
        {
            var file = new TextAsset("k#ey,english,german\nkey,a,\"b");
            var sut = new LanguageFileReader();

            Assert.Throws<ArgumentException>(() => sut.ReadTranslationMap(new[] { file }));
        }
    }
}
