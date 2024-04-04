using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Playmode.Tests.Extensions
{
    public class TimeExtensionsTest
    {
        [Test]
        public void ShouldElapseAfterGivenDuration()
        {
            Assert.False(1f.HasElapsed(Time.time));
            Assert.True(1f.HasElapsed(Time.time - 1.0001f));
            Assert.True(1f.HasElapsed(Time.time - 100f));
        }

        [Test]
        public void ShouldUseUnscaledTimeForHasElapsed()
        {
            Assert.False(1f.HasElapsed(Time.unscaledTime, true));
            Assert.True(1f.HasElapsed(Time.unscaledTime - 1.0001f, true));
        }
    }
}
