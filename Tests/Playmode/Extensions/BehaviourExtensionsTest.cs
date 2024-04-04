using System.Collections;
using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Extensions
{
    public class BehaviourExtensionsTest
    {
        [UnityTest]
        public IEnumerator ShouldDestroyGameObjectInPlayMode()
        {
            var behaviour = new GameObject().AddComponent<AudioService>();
            behaviour.DestroyGameObject();
            yield return null;
            Assert.False(behaviour);
        }
    }
}
