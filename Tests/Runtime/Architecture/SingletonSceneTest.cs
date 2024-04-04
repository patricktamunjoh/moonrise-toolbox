using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Audio;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Utilities.Assertions
{
    public class SingletonSceneTest
    {
        [Test]
        public void ShouldThrowIfNoInstanceAvailable()
        {
            Assert.Throws<SingletonNotAvailableException>(() => SingletonScene<AudioService>.Unit.Clear());
        }
    }
}
