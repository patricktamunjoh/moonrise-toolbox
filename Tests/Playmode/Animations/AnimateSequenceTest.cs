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
    public class AnimateSequenceTest
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
        public IEnumerator ShouldPlayAnimationsForLoop()
        {
            var startIndices = new List<int>();

            void InsertStartIndex(float progress, int index)
            {
                if (progress == 0)
                    startIndices.Add(index);
            }

            var animate01 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), p => InsertStartIndex(p, 0));
            var animate02 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), p => InsertStartIndex(p, 1));
            var sut = new AnimateSequence(AnimationPlayback.Loop, animate01, animate02);

            sut.Start();
            yield return new WaitForSeconds(0.1f);

            Assert.True(startIndices.Take(6).SequenceEqual(new[] { 0, 1, 0, 1, 0, 1 }));
        }

        [UnityTest]
        public IEnumerator ShouldPlayAnimationsForPingPong()
        {
            var startIndices = new List<int>();

            void InsertStartIndex(float progress, int index)
            {
                if (progress == 0)
                    startIndices.Add(index);
            }

            var animate01 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), p => InsertStartIndex(p, 0));
            var animate02 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), p => InsertStartIndex(p, 1));
            var sut = new AnimateSequence(AnimationPlayback.PingPong, animate01, animate02);

            sut.Start();
            yield return new WaitForSeconds(0.1f);

            Assert.True(startIndices.Take(6).SequenceEqual(new[] { 0, 1, 1, 0, 0, 1 }));
        }

        [UnityTest]
        public IEnumerator ShouldNotInvokeOnCompleteForLoop()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(AnimationPlayback.Loop, animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSeconds(0.1f);

            Assert.False(isInvoked);
        }

        [UnityTest]
        public IEnumerator ShouldNotInvokeOnCompleteForPingPong()
        {
            var isInvoked = false;
            var animate01 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), null);
            var animate02 = new AnimateBetween(Context, 0.01f, new AnimationConfigBuilder().Build(), null);
            var sut = new AnimateSequence(AnimationPlayback.PingPong, animate01, animate02);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            yield return new WaitForSeconds(0.1f);

            Assert.False(isInvoked);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
