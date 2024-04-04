using System.Collections;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Working
{
    public class WorkAlwaysTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Time.timeScale = 1;
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [UnityTest]
        public IEnumerator ShouldProvideDeltaTime()
        {
            var sut = new WorkAlways(Context);

            Time.timeScale = 0.5f;

            sut.OnProgress += p => Assert.True(Time.deltaTime.IsSimilar(p));
            sut.Start();

            yield return new WaitForSecondsRealtime(.1f);
        }

        [UnityTest]
        public IEnumerator ShouldProvideUnscaledDeltaTime()
        {
            var sut = new WorkAlways(Context, true);

            Time.timeScale = 0.5f;

            sut.OnProgress += p => Assert.True(Time.unscaledDeltaTime.IsSimilar(p));
            sut.Start();

            yield return new WaitForSecondsRealtime(.1f);
        }

        [UnityTest]
        public IEnumerator ShouldNotRunWhenPaused()
        {
            var isInvoked = false;
            var sut = new WorkAlways(Context);

            sut.Start();
            sut.Pause();
            sut.OnProgress += _ => isInvoked = true;

            yield return new WaitForSecondsRealtime(.1f);

            Assert.False(isInvoked);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
