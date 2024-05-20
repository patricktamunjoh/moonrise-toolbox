using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Types
{
    public class SmoothVector2Test
    {
        [Test]
        public void ShouldRespectMaxVelocity()
        {
            var sut = new SmoothVector2(new Vector2(12, 34), SmoothingDuration.Long, 0);
            sut.Advance(new Vector2(34, 56));

            Assert.True(sut.Velocity == Vector2.zero);
        }

        [Test]
        public void ShouldAdvanceToGivenTarget()
        {
            var sut = new SmoothVector2(new Vector2(12, 34), 0f, 100);

            for (var i = 0; i < 1000; i++)
                sut.Advance(new Vector2(13, 35));

            Assert.True(sut.Value.IsSimilar(new Vector2(13, 35)));
        }
    }
}
