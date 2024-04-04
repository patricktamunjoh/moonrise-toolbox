using System.Collections;
using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Animations
{
    public class AnimateContinuousTest
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
        public IEnumerator ShouldInvokeOnProgressWithDeltaTime()
        {
            var config = new AnimationConfigBuilder().Build();
            var sut = new AnimateContinuous(Context, config, null, p => Assert.True(p.IsSimilar(Time.deltaTime)));

            sut.Start();

            yield return new WaitForSecondsRealtime(0.1f);
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithScaledTime()
        {
            var config = new AnimationConfigBuilder().Build();
            var sut = new AnimateContinuous(Context, config, null, Assert.Zero);

            Time.timeScale = 0;
            yield return null;

            sut.Start();

            yield return new WaitForSecondsRealtime(0.1f);
        }

        [UnityTest]
        public IEnumerator ShouldPlayWithUnscaledTime()
        {
            var config = new AnimationConfigBuilder().WithUnscaledTime().Build();
            var sut = new AnimateContinuous(Context, config, null, p => Assert.Greater(p, 0));

            Time.timeScale = 0;
            sut.Start();

            yield return new WaitForSecondsRealtime(0.1f);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
