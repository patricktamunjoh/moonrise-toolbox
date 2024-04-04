using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Validation;
using Moq;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Localization
{
    public class LocalizedStringTest
    {
        private Mock<ILocalizationService> Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Service = new Mock<ILocalizationService>();
            Service.Object.SetAsSingletonInstance();
        }

        [Test]
        public void ShouldProvideTranslationForKey()
        {
            var sut = new LocalizedString("key");
            Service.Setup(x => x.GetLocalization("key", It.IsAny<string>())).Returns("translation");

            Assert.AreEqual("translation", sut.Value);
        }

        [Test]
        public void ShouldProvideTranslationForToStringAndCast()
        {
            var sut = new LocalizedString("key");
            Service.Setup(x => x.GetLocalization("key", It.IsAny<string>())).Returns("translation");

            Assert.AreEqual("translation", sut.ToString());
            Assert.AreEqual("translation", (string)sut);
        }

        [Test]
        public void ShouldBeInvalidForBlankKey()
        {
            var sut = new LocalizedString("  ");
            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeInvalidForEmptyKey()
        {
            var sut = new LocalizedString("");
            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeInvalidForMissingTranslation()
        {
            var sut = new LocalizedString("key");
            Service.Setup(x => x.IsLocalizationAvailable(It.IsAny<string>())).Returns(false);

            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeInvalidForMissingLocalizationService()
        {
            var sut = new LocalizedString("key");
            (null as ILocalizationService).SetAsSingletonInstance();

            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeValidWithCorrectSetup()
        {
            var sut = new LocalizedString("key");
            Service.Setup(x => x.IsLocalizationAvailable("key")).Returns(true);

            sut.Validate();
        }
    }
}
