using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using Moq;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Test]
        public void ShouldProvideLocalization()
        {
            var service = new Mock<ILocalizationService>();

            service.Object.SetAsSingletonInstance();
            service.Setup(x => x.GetLocalization("key", It.IsAny<string>())).Returns("value");

            Assert.AreEqual("value", "key".Localized());
        }

        [Test]
        public void ShouldProvideLocalizationWithFallback()
        {
            var service = new Mock<ILocalizationService>();

            service.Object.SetAsSingletonInstance();
            service.Setup(x => x.GetLocalization(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((_, x) => x);

            Assert.AreEqual("fallback", "key".Localized("fallback"));
        }

        [Test]
        public void ShouldApplyTitleCase()
        {
            Assert.AreEqual("Example Text", "example text".TitleCase());
            Assert.AreEqual("", "".TitleCase());
            Assert.AreEqual("Example", "example".TitleCase());
            Assert.AreEqual("E X A M P L E", "e x a m p l e".TitleCase());
            Assert.AreEqual("Ex Am Ple", " ex  am  ple".TitleCase());
        }

        [Test]
        public void ShouldRemoveLineBreaksAndIndents()
        {
            const string sut =
                @"This string contains
                line breaks and gaps and is stretched over
                multiple lines";

            Assert.AreEqual("This string contains\r\nline breaks and gaps and is stretched over\r\nmultiple lines", sut.TrimIndents());
        }

        [Test]
        public void ShouldClampTextWithEllipsis()
        {
            Assert.AreEqual("example", "example".Clamped(7));
            Assert.AreEqual("example", "example".Clamped(100));
            Assert.AreEqual("", "example".Clamped(0));
            Assert.AreEqual("", "example".Clamped(-5));
            Assert.AreEqual("", "example".Clamped(0, "..."));
            Assert.AreEqual("exa...", "example".Clamped(6, "..."));
            Assert.AreEqual("e...", "example".Clamped(4, "..."));
        }
    }
}
