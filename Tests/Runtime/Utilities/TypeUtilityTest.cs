using System.Linq;
using MoonriseGames.Toolbox.Utilities;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Utilities
{
    public class TypeUtilityTest
    {
        [Test]
        public void ShouldReturnEnumValues()
        {
            var sut = TypeUtility.GetEnumValues<SampleEnum>();
            Assert.True(sut.SequenceEqual(new[] { SampleEnum.One, SampleEnum.Two, SampleEnum.Three }));
        }

        private enum SampleEnum
        {
            One,
            Two,
            Three
        }
    }
}
