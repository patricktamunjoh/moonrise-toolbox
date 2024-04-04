#region

using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;

#endregion
namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class BehaviourExtensionsTest
    {
        [Test]
        public void ShouldHandleNullBehaviour()
        {
            (null as MonoBehaviour).DestroyGameObject();
        }

        [Test]
        public void ShouldHandleDestroyedBehaviour()
        {
            var behaviour = new GameObject().AddComponent<AudioService>();
            Object.DestroyImmediate(behaviour.gameObject);
            behaviour.DestroyGameObject();
        }

        [Test]
        public void ShouldDestroyGameObjectInEditMode()
        {
            var behaviour = new GameObject().AddComponent<AudioService>();
            behaviour.DestroyGameObject();
            Assert.False(behaviour);
        }
    }
}
