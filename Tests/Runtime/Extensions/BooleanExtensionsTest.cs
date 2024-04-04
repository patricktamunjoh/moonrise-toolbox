using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class BooleanExtensionsTest
    {
        [Test]
        public void ShouldInvertBoolean()
        {
            Assert.True(false.Not());
            Assert.False(true.Not());
        }
    }
}
