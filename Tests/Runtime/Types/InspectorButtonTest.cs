using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Types
{
    public class InspectorButtonTest
    {
        [Test]
        public void ShouldNotBeSetByDefault()
        {
            var sut = new InspectorButton();
            Assert.False(sut.IsClicked);
            Assert.False(sut.IsToggled);
        }

        [Test]
        public void ShouldResetAfterClickCheck()
        {
            var sut = new InspectorButton();
            sut.SetNonPublicField(true);

            Assert.True(sut.IsClicked);
            Assert.False(sut.IsClicked);
        }

        [Test]
        public void ShouldNotResetAfterToggleCheck()
        {
            var sut = new InspectorButton();
            sut.SetNonPublicField(true);

            Assert.True(sut.IsToggled);
            Assert.True(sut.IsToggled);
        }
    }
}
