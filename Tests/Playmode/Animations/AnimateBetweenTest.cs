using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Animations
{
    public class AnimateBetweenTest
    {
        private SampleBehaviour Context { get; set; }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            Function.ClearScene();
            Time.timeScale = 1;
            Context = new GameObject().AddComponent<SampleBehaviour>();
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldNotInvokeOnCompleteForLoop()
        {
            var isInvoked = false;
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.Loop).Build();
            var sut = new AnimateBetween(Context, 0.1f, config, null);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSecondsRealtime(0.2f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldNotInvokeOnCompleteForPingPong()
        {
            var isInvoked = false;
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.PingPong).Build();
            var sut = new AnimateBetween(Context, 0.1f, config, null);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSecondsRealtime(0.2f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithPingPong()
        {
            var progress = new List<float>();
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.PingPong).Build();
            var sut = new AnimateBetween(Context, 0.1f, config, p => progress.Add(p));

            sut.Start();

            yield return new WaitForSecondsRealtime(0.3f);

            Assert.Greater(progress.Count, 10);
            Assert.Less(progress.Zip(progress.Skip(1), (a, b) => Mathf.Abs(a - b)).Max(), 0.5f);
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithLoop()
        {
            var progress = new List<float>();
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.Loop).Build();
            var sut = new AnimateBetween(Context, 0.1f, config, p => progress.Add(p));

            sut.Start();

            yield return new WaitForSecondsRealtime(0.2f);

            Assert.Greater(progress.Count, 10);
            Assert.AreEqual(1, progress.Zip(progress.Skip(1), (a, b) => Mathf.Abs(a - b)).Max());
            Assert.True(progress.Zip(progress.Skip(1), (a, b) => a < b || a == 1).All(x => x));
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithScaledTime()
        {
            var config = new AnimationConfigBuilder().Build();
            var sut = new AnimateBetween(Context, 0.1f, config, null);

            Time.timeScale = 0;
            sut.Start();

            yield return new WaitForSecondsRealtime(0.2f);

            Assert.True(sut.IsStarted);
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithUnscaledTime()
        {
            var config = new AnimationConfigBuilder().WithUnscaledTime().Build();
            var sut = new AnimateBetween(Context, 0.1f, config, null);

            Time.timeScale = 0;
            sut.Start();

            yield return new WaitForSecondsRealtime(0.2f);

            Assert.False(sut.IsStarted);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
