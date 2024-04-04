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
    public class SingletonSceneTest
    {
        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            (null as SingletonSceneSample).SetAsSingletonInstance();
        }

        [UnityTest]
        public IEnumerator ShouldCallInstanceLifecycleMethods()
        {
            var sut = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;

            Object.Destroy(sut.gameObject);
            yield return null;

            Assert.True(sut.IsAwakeCalled);
            Assert.True(sut.IsDestroyCalled);
        }

        [UnityTest]
        public IEnumerator ShouldFindInstance()
        {
            var sut01 = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;

            Assert.AreEqual(sut01, SingletonSceneSample.Unit);
            Object.Destroy(sut01.gameObject);
            yield return null;

            var sut02 = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;

            Assert.AreEqual(sut02, SingletonSceneSample.Unit);
            Object.Destroy(sut02.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldCallInstanceLifecycleMethodsOnlyForInstance()
        {
            var instance = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;
            Assert.AreEqual(instance, SingletonSceneSample.Unit);

            var sut = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;

            Object.Destroy(instance.gameObject);
            yield return null;

            Assert.False(sut.IsAwakeCalled);
            Assert.False(sut.IsDestroyCalled);
        }

        [UnityTest]
        public IEnumerator ShouldDestroyAdditionalInstances()
        {
            var instance = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;
            Assert.AreEqual(instance, SingletonSceneSample.Unit);

            var sut = new GameObject().AddComponent<SingletonSceneSample>();
            yield return null;

            Object.Destroy(instance.gameObject);
            yield return null;

            Assert.False(sut);
        }

        public class SingletonSceneSample : SingletonScene<SingletonSceneSample>
        {
            public bool IsAwakeCalled { get; private set; }
            public bool IsDestroyCalled { get; private set; }

            protected override void AwakeInstance()
            {
                base.AwakeInstance();
                IsAwakeCalled = true;
            }

            protected override void OnDestroyInstance()
            {
                base.OnDestroyInstance();
                IsDestroyCalled = true;
            }
        }
    }
}
