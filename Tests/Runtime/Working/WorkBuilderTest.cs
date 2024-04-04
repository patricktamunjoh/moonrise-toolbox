using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Working
{
    public class WorkBuilderTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldBuildWorkOnce()
        {
            var work = new WorkBuilder(Context, 10).Build();
            Assert.AreEqual(typeof(WorkOnce), work.GetType());
        }

        [Test]
        public void ShouldBuildWorkAlways()
        {
            var work = new WorkBuilder(Context).Build();
            Assert.AreEqual(typeof(WorkAlways), work.GetType());
        }

        [Test]
        public void ShouldConfigureOnProgressAction()
        {
            var invocations = 0;
            new WorkBuilder(Context, 10).OnProgress(_ => invocations++).OnProgress(_ => invocations++).Build().Start();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldConfigureOnCompleteAction()
        {
            var invocations = 0;
            new WorkBuilder(Context, 0).OnComplete(() => invocations++).OnComplete(() => invocations++).Build().Start();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldConfigureOnStartAction()
        {
            var invocations = 0;
            new WorkBuilder(Context, 10).OnStart(() => invocations++).OnStart(() => invocations++).Build().Start();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldConfigureOnStopAction()
        {
            var invocations = 0;
            var work = new WorkBuilder(Context, 10).OnStop(() => invocations++).OnStop(() => invocations++).Build();

            work.Start();
            work.Stop();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldConfigureOnPauseAction()
        {
            var invocations = 0;
            var work = new WorkBuilder(Context, 10).OnPause(() => invocations++).OnPause(() => invocations++).Build();

            work.Start();
            work.Pause();

            Assert.AreEqual(2, invocations);
        }

        [Test]
        public void ShouldConfigureOnResumeAction()
        {
            var invocations = 0;
            var work = new WorkBuilder(Context, 10).OnResume(() => invocations++).OnResume(() => invocations++).Build();

            work.Start();
            work.Pause();
            work.Resume();

            Assert.AreEqual(2, invocations);
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
