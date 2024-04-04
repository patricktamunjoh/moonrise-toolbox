using MoonriseGames.Toolbox.Localization;
using MoonriseGames.Toolbox.Messaging;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using Moq;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Messaging
{
    public class MessageTest
    {
        [Test]
        public void ShouldProvideDescription()
        {
            var service = new Mock<ILocalizationService>();

            service.Object.SetAsSingletonInstance();
            service.Setup(x => x.GetLocalization("key", It.IsAny<string>())).Returns("example");

            var sut = new MessageWithDescription();

            Assert.AreEqual("example", sut.Description);
        }

        [Test]
        public void ShouldProvideNullWithoutDescription()
        {
            var sut = new MessageWithoutDescription();
            Assert.Null(sut.Description);
        }

        private class MessageWithDescription : Message
        {
            protected override string DescriptionKey => "key";
        }

        private class MessageWithoutDescription : Message { }
    }
}
