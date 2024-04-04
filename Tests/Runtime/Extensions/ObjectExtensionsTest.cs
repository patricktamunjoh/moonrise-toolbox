using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class ObjectExtensionsTest
    {
        [Test]
        public void ShouldDetectNullObject()
        {
            Assert.True((null as string).IsNull());
            Assert.True((null as Object).IsNull());
        }

        [Test]
        public void ShouldDetectPseudoNullObject()
        {
            var go = new GameObject();

            Object.DestroyImmediate(go);
            Assert.True(go.IsNull());
        }

        [Test]
        public void ShouldDetectNoneNullObject()
        {
            var go = new GameObject();

            Assert.False(go.IsNull());
            Assert.False(12.IsNull());
            Assert.False("example".IsNull());
        }
    }
}
