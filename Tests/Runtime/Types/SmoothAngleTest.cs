using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Types
{
    public class SmoothAngleTest
    {
        [Test]
        public void ShouldRespectMaxVelocity()
        {
            var sut = new SmoothAngle(12, SmoothingDuration.Long, 0);
            sut.Advance(15);

            Assert.Zero(sut.Velocity);
        }

        [Test]
        public void ShouldAdvanceToGivenTarget()
        {
            var sut = new SmoothAngle(12, 0f, 100);

            for (var i = 0; i < 100; i++)
                sut.Advance(13);
            Assert.True(sut.Value.IsSimilar(13));
        }

        [Test]
        public void ShouldUseAngleInterpolation()
        {
            var sut = new SmoothAngle(360, 0.2f, 100);
            var points = new List<float>();

            for (var i = 0; i < 100; i++)
                points.Add(sut.Advance(5));

            Assert.True(points.All(x => x is >= 360 or <= 5));
        }
    }
}
