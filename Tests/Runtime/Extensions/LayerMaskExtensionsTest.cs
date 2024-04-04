using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class LayerMaskExtensionsTest
    {
        [Test]
        public void ShouldFindContainedLayer()
        {
            var sut = (LayerMask)(1 << 3);

            Assert.True(sut.Contains(3));
            Assert.False(sut.Contains(0));
            Assert.False(sut.Contains(4));
        }
    }
}
