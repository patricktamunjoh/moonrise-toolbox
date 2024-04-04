using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Working
{
    public class WorkOnceTest
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
        public IEnumerator ShouldAdvanceProgress()
        {
            var progress = new List<float>();
            var sut = new WorkOnce(Context, 1);

            sut.OnProgress += p => progress.Add(p);
            sut.Start();

            yield return new WaitForSecondsRealtime(.1f);

            Assert.True(progress.Zip(progress.Skip(1), (a, b) => a < b).All(x => x));
        }

        [UnityTest]
        public IEnumerator ShouldExecuteInBehaviourContext()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.OnProgress += _ => isInvoked = true;
            Object.Destroy(Context);

            yield return new WaitForSecondsRealtime(.1f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldStopAfterCompletion()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0);

            sut.Start();
            sut.OnProgress += _ => isInvoked = true;

            yield return new WaitForSecondsRealtime(.1f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldRunForSetDuration()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0.1f);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);

            yield return new WaitForSecondsRealtime(.2f);

            Assert.True(isInvoked);
        }

        [UnityTest, Ignore("Cannot detect coroutine status from enumerator")]
        public IEnumerator ShouldRestartAfterReactivateContext()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0.1f);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return null;
            Context.gameObject.SetActive(false);

            yield return null;
            Context.gameObject.SetActive(true);
            sut.Start();

            yield return new WaitForSecondsRealtime(.2f);

            Assert.True(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldUseScaledDeltaTime()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0.1f);

            Time.timeScale = 0;

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSecondsRealtime(.2f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldUseUnscaledDeltaTime()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0.1f, true);

            Time.timeScale = 0;

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSecondsRealtime(.2f);

            Assert.True(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldNotRunWhenPaused()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0.1f);

            sut.Start();
            sut.Pause();
            sut.OnProgress += _ => isInvoked = true;

            yield return new WaitForSecondsRealtime(.1f);

            Assert.False(isInvoked);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
