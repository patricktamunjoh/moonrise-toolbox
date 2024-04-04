using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class EnumerationExtensionsTest
    {
        [Test]
        public void ShouldIndexElementsZeroBased()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5 };
            var sut = array.Indexed().ToArray();

            Assert.True(sut.All(x => x.value == x.index));
        }
    }
}
