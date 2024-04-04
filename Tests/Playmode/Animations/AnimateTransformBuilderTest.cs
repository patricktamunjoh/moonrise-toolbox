using System.Collections;
using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using MoonriseGames.Toolbox.Types;
using MoonriseGames.Toolbox.Working;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MoonriseGames.Toolbox.Playmode.Tests.Animations
{
    public class AnimateTransformBuilderTest
    {
        private SampleBehaviour Context { get; set; }

        private Transform Transform { get; set; }
        private Transform Parent { get; set; }

        private AnimationConfig Config { get; } = new AnimationConfigBuilder().Build();

        [UnitySetUp]
        public IEnumerator Setup()
        {
            Function.ClearScene();
            Time.timeScale = 1;

            Context = new GameObject().AddComponent<SampleBehaviour>();
            Parent = new GameObject().transform;
            Transform = Context.transform;

            Context.transform.parent = Parent;

            yield return null;
        }

        [UnityTest]
        public IEnumerator ShouldUseLastDelayDuration()
        {
            new AnimateTransformBuilder(Context).MoveTo(0, Config, new Vector3(2, 2, 2)).Delay(10).Delay(0.1f).Build().Start();
            yield return new WaitForSecondsRealtime(0.2f);

            Assert.True(new Vector3(2, 2, 2).IsSimilar(Context.transform.position));
        }

        [UnityTest]
        public IEnumerator ShouldRotateToAngleUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateTo(0.1f, Config, 300).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.z))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateToLocalAngleUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateToLocal(0.1f, Config, 300).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.z)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateBetweenAnglesUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).Rotate(0.1f, Config, 0, 300).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.z))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateBetweenLocalAnglesUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateLocal(0.1f, Config, 0, 300).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.z)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateToEulerUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateTo(0.1f, Config, new Vector3(0, 300, 0)).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.y))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateToLocalEulerUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateToLocal(0.1f, Config, new Vector3(0, 300, 0)).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.y)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateBetweenEulerUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).Rotate(0.1f, Config, Vector3.zero, new Vector3(0, 300, 0)).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.y))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateBetweenLocalEulerUsingAngleInterpolation()
        {
            var interval = new Interval(300, 360);
            new AnimateTransformBuilder(Context).RotateLocal(0.1f, Config, Vector3.zero, new Vector3(0, 300, 0)).Build().Start();

            yield return null;
            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.y)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateByAngleUsingEulerInterpolation()
        {
            var interval = new Interval(0, 300);
            new AnimateTransformBuilder(Context).RotateBy(0.1f, Config, 300).Build().Start();

            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.z))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateByLocalAngleUsingEulerInterpolation()
        {
            var interval = new Interval(0, 300);
            new AnimateTransformBuilder(Context).RotateByLocal(0.1f, Config, 300).Build().Start();

            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.z)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateByEulerUsingEulerInterpolation()
        {
            var interval = new Interval(0, 300);
            new AnimateTransformBuilder(Context).RotateBy(0.1f, Config, new Vector3(0, 300, 0)).Build().Start();

            new WorkBuilder(Context, 0.1f).OnProgress(_ => Assert.True(interval.IsInside(Context.transform.eulerAngles.y))).Build().Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateByLocalEulerUsingEulerInterpolation()
        {
            var interval = new Interval(0, 300);
            new AnimateTransformBuilder(Context).RotateByLocal(0.1f, Config, new Vector3(0, 300, 0)).Build().Start();

            new WorkBuilder(Context, 0.1f)
                .OnProgress(_ => Assert.True(interval.IsInside(Context.transform.localEulerAngles.y)))
                .Build()
                .Start();

            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator ShouldRotateContinuouslyByFloatVelocity()
        {
            new AnimateTransformBuilder(Context).Rotate(Config, 10).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.eulerAngles.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldRotateLocalContinuouslyByFloatVelocity()
        {
            new AnimateTransformBuilder(Context).RotateLocal(Config, 10).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.localEulerAngles.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldRotateContinuouslyByEulerVelocity()
        {
            new AnimateTransformBuilder(Context).Rotate(Config, Vector3.one).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.eulerAngles.x, 0);
            Assert.Greater(Context.transform.eulerAngles.y, 0);
            Assert.Greater(Context.transform.eulerAngles.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldRotateLocalContinuouslyByEulerVelocity()
        {
            new AnimateTransformBuilder(Context).RotateLocal(Config, Vector3.one).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.localEulerAngles.x, 0);
            Assert.Greater(Context.transform.localEulerAngles.y, 0);
            Assert.Greater(Context.transform.localEulerAngles.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldResetToOriginalRotationForContinuousRotation()
        {
            Context.transform.eulerAngles = new Vector3(10, 30, 50);
            var animation = new AnimateTransformBuilder(Context).Rotate(Config, Vector3.one).Build();

            animation.Start();
            yield return null;

            animation.Reset();
            Assert.True(new Vector3(10, 30, 50).IsSimilar(Context.transform.eulerAngles));
        }

        [UnityTest]
        public IEnumerator ShouldResetToOriginalLocalRotationForContinuousRotation()
        {
            Parent.transform.eulerAngles = new Vector3(20, 40, 10);
            Context.transform.localEulerAngles = new Vector3(10, 30, 50);

            var animation = new AnimateTransformBuilder(Context).RotateLocal(Config, Vector3.one).Build();

            animation.Start();
            yield return null;

            animation.Reset();
            Assert.True(new Vector3(10, 30, 50).IsSimilar(Context.transform.localEulerAngles));
        }

        [UnityTest]
        public IEnumerator ShouldMoveContinuously()
        {
            new AnimateTransformBuilder(Context).Move(Config, Vector3.one).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.position.x, 0);
            Assert.Greater(Context.transform.position.y, 0);
            Assert.Greater(Context.transform.position.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldMoveLocalContinuously()
        {
            new AnimateTransformBuilder(Context).MoveLocal(Config, Vector3.one).Build().Start();
            yield return null;

            Assert.Greater(Context.transform.localPosition.x, 0);
            Assert.Greater(Context.transform.localPosition.y, 0);
            Assert.Greater(Context.transform.localPosition.z, 0);
        }

        [UnityTest]
        public IEnumerator ShouldResetToOriginalPositionForContinuousPosition()
        {
            Context.transform.position = new Vector3(1, 2, 3);
            var animation = new AnimateTransformBuilder(Context).Move(Config, Vector3.one).Build();

            animation.Start();
            yield return null;

            animation.Reset();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));
        }

        [UnityTest]
        public IEnumerator ShouldResetToOriginalLocalPositionForContinuousPosition()
        {
            Context.transform.localPosition = new Vector3(1, 2, 3);
            var animation = new AnimateTransformBuilder(Context).MoveLocal(Config, Vector3.one).Build();

            animation.Start();
            yield return null;

            animation.Reset();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.localPosition));
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
