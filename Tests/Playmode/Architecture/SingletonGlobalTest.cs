using System.Collections;
using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace MoonriseGames.Toolbox.Playmode.Tests.Architecture
{
    public class SingletonGlobalTest
    {
        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            (null as SingletonGlobalSample).SetAsSingletonInstance();
        }

        [UnityTest]
        public IEnumerator ShouldNotBeDestroyedOnLoad()
        {
            var sut = new GameObject().AddComponent<SingletonGlobalSample>();
            yield return null;

            Assert.AreEqual(sut.gameObject.scene.buildIndex, -1);
            Object.Destroy(sut.gameObject);
            yield return null;
        }

        public class SingletonGlobalSample : SingletonGlobal<SingletonGlobalSample> { }
    }
}
