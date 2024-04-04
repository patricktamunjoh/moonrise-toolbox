using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Working
{
    public class WorkOnceTest
    {
        private SampleBehaviour Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Context = new GameObject().AddComponent<SampleBehaviour>();
        }

        [Test]
        public void ShouldHandleNullContextAtStart()
        {
            var sut = new WorkOnce(null, 1);
            sut.Start();
        }

        [Test]
        public void ShouldHandleNullContextAtStop()
        {
            var sut = new WorkOnce(Context, 1);
            sut.Start();

            Object.DestroyImmediate(Context);
            sut.Stop();
        }

        [Test]
        public void ShouldHandleNullContextAtRestart()
        {
            var sut = new WorkOnce(Context, 1);
            sut.Start();

            Object.DestroyImmediate(Context);
            sut.Restart();
        }

        [Test]
        public void ShouldBeStartedAfterStart()
        {
            var sut = new WorkOnce(Context, 1);
            sut.Start();

            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldBeStartedAfterPauseAndResume()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();

            Assert.True(sut.IsStarted);

            sut.Resume();
            Assert.True(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBeStartedInitially()
        {
            var sut = new WorkOnce(Context, 1);
            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBeStartedAfterStop()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Stop();

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldNotBeStartedAfterCompletion()
        {
            var sut = new WorkOnce(Context, 0);
            sut.Start();

            Assert.False(sut.IsStarted);
        }

        [Test]
        public void ShouldBePausedAfterPause()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();

            Assert.True(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedAfterResume()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.Resume();

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedInitially()
        {
            var sut = new WorkOnce(Context, 1);
            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedAfterStop()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.Stop();

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldNotBePausedAfterRestart()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.Restart();

            Assert.False(sut.IsPaused);
        }

        [Test]
        public void ShouldInvokeOnProgress()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnProgress += _ => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnProgressWithZero()
        {
            var sut = new WorkOnce(Context, 1);

            sut.OnProgress += p => Assert.AreEqual(0, p);
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnProgressWithOne()
        {
            var sut = new WorkOnce(Context, 0);

            sut.OnProgress += p => Assert.AreEqual(1, p);
            sut.Start();
        }

        [Test]
        public void ShouldInvokeOnStart()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnStart += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnStartForRestart()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnStart += () => isInvoked = true;
            sut.Restart();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnStop()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.OnStop += () => isInvoked = true;
            sut.Stop();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldNotInvokeOnStopForRestart()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnStop += () => isInvoked = true;
            sut.Start();
            sut.Restart();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnPause()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.OnPause += () => isInvoked = true;
            sut.Pause();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnResume()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.OnResume += () => isInvoked = true;
            sut.Resume();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldInvokeOnComplete()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0);

            sut.OnComplete += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldStartOnlyWhenStopped()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.OnStart += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldNotStartForInactiveContext()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            Context.gameObject.SetActive(false);

            sut.OnStart += () => isInvoked = true;
            sut.Start();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldStartAfterCompletion()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 0);

            sut.Start();
            sut.OnStart += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldResumeWhenStartingWhilePaused()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.OnResume += () => isInvoked = true;
            sut.Start();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldStopOnlyWhenStarted()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnStop += () => isInvoked = true;
            sut.Stop();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldStopWhenPaused()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.OnStop += () => isInvoked = true;
            sut.Stop();

            Assert.True(isInvoked);
        }

        [Test]
        public void ShouldPauseOnlyWhenStarted()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnPause += () => isInvoked = true;
            sut.Pause();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldPauseOnlyOnce()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.OnPause += () => isInvoked = true;
            sut.Pause();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldResumeOnlyWhenStarted()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.OnResume += () => isInvoked = true;
            sut.Resume();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldResumeOnlyOnce()
        {
            var isInvoked = false;
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.Pause();
            sut.Resume();
            sut.OnResume += () => isInvoked = true;
            sut.Resume();

            Assert.False(isInvoked);
        }

        [Test]
        public void ShouldRestartFromBeginning()
        {
            var sut = new WorkOnce(Context, 1);

            sut.Start();
            sut.OnProgress += p => Assert.AreEqual(0, p);
            sut.Restart();
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
