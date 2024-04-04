using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateTransformBuilderTest
    {
        private SampleBehaviour Context { get; set; }
        private Transform Parent { get; set; }

        private AnimationConfig Config { get; } = new AnimationConfigBuilder().Build();

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();

            Context = new GameObject().AddComponent<SampleBehaviour>();
            Parent = new GameObject().transform;

            Context.transform.parent = Parent;
        }

        [Test]
        public void ShouldUseProvidedTransform()
        {
            var transform = new GameObject().transform;
            new AnimateTransformBuilder(transform, Context).MoveTo(0, Config, new Vector3(1, 2, 3)).Build().Start();

            Assert.True(new Vector3(1, 2, 3).IsSimilar(transform.position));
            Assert.True(Vector3.zero.IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldClearAnimationsAfterBuild()
        {
            var sut = new AnimateTransformBuilder(Context).MoveTo(0, Config, new Vector3(1, 2, 3));

            Assert.NotNull(sut.Build());
            Assert.Null(sut.Build());
        }

        [Test]
        public void ShouldReturnNullForEmptyAnimation()
        {
            var sut = new AnimateTransformBuilder(Context);
            Assert.Null(sut.Build());
        }

        [Test]
        public void ShouldBuildSingleAnimation()
        {
            var sut = new AnimateTransformBuilder(Context).MoveTo(0, Config, new Vector3(1, 2, 3));
            Assert.IsNotInstanceOf<AnimateTogether>(sut.Build());
        }

        [Test]
        public void ShouldBuildCombinedAnimation()
        {
            var animation = new AnimateTransformBuilder(Context)
                .MoveTo(0, Config, new Vector3(1, 2, 3))
                .ScaleTo(0, Config, new Vector3(3, 2, 1))
                .Build();

            animation.Start();

            Assert.IsInstanceOf<AnimateTogether>(animation);

            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));
            Assert.True(new Vector3(3, 2, 1).IsSimilar(Context.transform.localScale));
        }

        [Test]
        public void ShouldBuildAnimationSequence()
        {
            var animation = new AnimateTransformBuilder(Context)
                .MoveTo(0, Config, new Vector3(1, 2, 3))
                .MoveTo(0, Config, new Vector3(3, 2, 1))
                .Build(AnimationPlayback.Forward);

            animation.Start();

            Assert.IsInstanceOf<AnimateSequence>(animation);
            Assert.True(new Vector3(3, 2, 1).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldAddDelayBeforeAnimations()
        {
            new AnimateTransformBuilder(Context).Delay(1).MoveTo(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(Vector3.zero.IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldNotAddNegativeDelayBeforeAnimations()
        {
            new AnimateTransformBuilder(Context).Delay(-1).MoveTo(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldAddDelayBetweenAnimations()
        {
            new AnimateTransformBuilder(Context)
                .MoveTo(0, Config, new Vector3(1, 2, 3))
                .Delay(1)
                .MoveTo(0, Config, new Vector3(8, 8, 8))
                .Build(AnimationPlayback.Forward)
                .Start();

            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldScaleToVector()
        {
            Context.transform.localScale = new Vector3(2, 4, 4);

            new AnimateTransformBuilder(Context).ScaleTo(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.localScale));

            new AnimateTransformBuilder(Context).ScaleTo(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.localScale));
        }

        [Test]
        public void ShouldScaleByVector()
        {
            Context.transform.localScale = new Vector3(2, 4, 4);

            new AnimateTransformBuilder(Context).ScaleBy(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.localScale));

            new AnimateTransformBuilder(Context).ScaleBy(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(3, 6, 7).IsSimilar(Context.transform.localScale));
        }

        [Test]
        public void ShouldScaleBetweenVectors()
        {
            Context.transform.localScale = new Vector3(2, 4, 4);

            new AnimateTransformBuilder(Context).Scale(1, Config, Vector3.zero, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(Vector3.zero.IsSimilar(Context.transform.localScale));

            new AnimateTransformBuilder(Context).Scale(0, Config, Vector3.zero, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.localScale));
        }

        [Test]
        public void ShouldResetToOriginalScale()
        {
            Context.transform.localScale = new Vector3(2, 4, 4);

            var animation = new AnimateTransformBuilder(Context).ScaleBy(0, Config, new Vector3(1, 2, 3)).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.localScale));
        }

        [Test]
        public void ShouldRotateToAngle()
        {
            Context.transform.eulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).RotateTo(1, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 25).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).RotateTo(0, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 90).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateToLocalAngle()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).RotateToLocal(1, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 25).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateToLocal(0, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 90).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldRotateByAngle()
        {
            Context.transform.eulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).RotateBy(1, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 25).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).RotateBy(0, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 115).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateByLocalAngle()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).RotateByLocal(1, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 25).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateByLocal(0, Config, 90).Build().Start();
            Assert.True(new Vector3(0, 0, 115).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldRotateBetweenAngles()
        {
            Context.transform.eulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).Rotate(1, Config, 90, 180).Build().Start();
            Assert.True(new Vector3(0, 0, 90).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).Rotate(0, Config, 90, 180).Build().Start();
            Assert.True(new Vector3(0, 0, 180).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateBetweenLocalAngles()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(0, 0, 25);

            new AnimateTransformBuilder(Context).RotateLocal(1, Config, 90, 180).Build().Start();
            Assert.True(new Vector3(0, 0, 90).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateLocal(0, Config, 90, 180).Build().Start();
            Assert.True(new Vector3(0, 0, 180).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldRotateToEuler()
        {
            Context.transform.eulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).RotateTo(1, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(60, 85, 25).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).RotateTo(0, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(10, 20, 30).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateToLocalEuler()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).RotateToLocal(1, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(60, 85, 25).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateToLocal(0, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(10, 20, 30).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldRotateByEuler()
        {
            Context.transform.eulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).RotateBy(1, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(60, 85, 25).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).RotateBy(0, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(70, 105, 55).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateByLocalEuler()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).RotateByLocal(1, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(60, 85, 25).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateByLocal(0, Config, new Vector3(10, 20, 30)).Build().Start();
            Assert.True(new Vector3(70, 105, 55).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldRotateBetweenEuler()
        {
            Context.transform.eulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).Rotate(1, Config, new Vector3(10, 20, 30), new Vector3(5, 10, 30)).Build().Start();
            Assert.True(new Vector3(10, 20, 30).IsSimilar(Context.transform.eulerAngles));

            new AnimateTransformBuilder(Context).Rotate(0, Config, new Vector3(10, 20, 30), new Vector3(5, 10, 30)).Build().Start();
            Assert.True(new Vector3(5, 10, 30).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldRotateBetweenLocalEuler()
        {
            Parent.transform.eulerAngles = new Vector3(10, 40, 240);
            Context.transform.localEulerAngles = new Vector3(60, 85, 25);

            new AnimateTransformBuilder(Context).RotateLocal(1, Config, new Vector3(10, 20, 30), new Vector3(5, 10, 30)).Build().Start();
            Assert.True(new Vector3(10, 20, 30).IsSimilar(Context.transform.localEulerAngles));

            new AnimateTransformBuilder(Context).RotateLocal(0, Config, new Vector3(10, 20, 30), new Vector3(5, 10, 30)).Build().Start();
            Assert.True(new Vector3(5, 10, 30).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldResetToOriginalRotation()
        {
            Context.transform.eulerAngles = new Vector3(2, 4, 4);

            var animation = new AnimateTransformBuilder(Context).RotateBy(0, Config, new Vector3(1, 2, 3)).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.eulerAngles));
        }

        [Test]
        public void ShouldResetToOriginalLocalRotation()
        {
            Parent.transform.eulerAngles = new Vector3(3, 6, 9);
            Context.transform.localEulerAngles = new Vector3(2, 4, 4);

            var animation = new AnimateTransformBuilder(Context).RotateByLocal(0, Config, new Vector3(1, 2, 3)).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.localEulerAngles));
        }

        [Test]
        public void ShouldMoveToPosition()
        {
            Context.transform.position = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).MoveTo(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(3, 4, 5).IsSimilar(Context.transform.position));

            new AnimateTransformBuilder(Context).MoveTo(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldMoveToLocalPosition()
        {
            Parent.transform.position = new Vector3(2, 8, 5);
            Context.transform.localPosition = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).MoveToLocal(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(3, 4, 5).IsSimilar(Context.transform.localPosition));

            new AnimateTransformBuilder(Context).MoveToLocal(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.localPosition));
        }

        [Test]
        public void ShouldMoveByVector()
        {
            Context.transform.position = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).MoveBy(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(3, 4, 5).IsSimilar(Context.transform.position));

            new AnimateTransformBuilder(Context).MoveBy(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(4, 6, 8).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldMoveByLocalVector()
        {
            Parent.transform.position = new Vector3(2, 8, 5);
            Context.transform.localPosition = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).MoveByLocal(1, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(3, 4, 5).IsSimilar(Context.transform.localPosition));

            new AnimateTransformBuilder(Context).MoveByLocal(0, Config, new Vector3(1, 2, 3)).Build().Start();
            Assert.True(new Vector3(4, 6, 8).IsSimilar(Context.transform.localPosition));
        }

        [Test]
        public void ShouldMoveBetweenPoints()
        {
            Context.transform.position = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).Move(1, Config, new Vector3(1, 2, 3), new Vector3(3, 1, 4)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.position));

            new AnimateTransformBuilder(Context).Move(0, Config, new Vector3(1, 2, 3), new Vector3(3, 1, 4)).Build().Start();
            Assert.True(new Vector3(3, 1, 4).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldMoveBetweenLocalPoints()
        {
            Parent.transform.position = new Vector3(2, 8, 5);
            Context.transform.localPosition = new Vector3(3, 4, 5);

            new AnimateTransformBuilder(Context).MoveLocal(1, Config, new Vector3(1, 2, 3), new Vector3(3, 1, 4)).Build().Start();
            Assert.True(new Vector3(1, 2, 3).IsSimilar(Context.transform.localPosition));

            new AnimateTransformBuilder(Context).MoveLocal(0, Config, new Vector3(1, 2, 3), new Vector3(3, 1, 4)).Build().Start();
            Assert.True(new Vector3(3, 1, 4).IsSimilar(Context.transform.localPosition));
        }

        [Test]
        public void ShouldResetToOriginalPosition()
        {
            Context.transform.position = new Vector3(2, 4, 4);

            var animation = new AnimateTransformBuilder(Context).MoveBy(0, Config, new Vector3(1, 2, 3)).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.position));
        }

        [Test]
        public void ShouldResetToOriginalLocalPosition()
        {
            Parent.transform.position = new Vector3(3, 6, 9);
            Context.transform.localPosition = new Vector3(2, 4, 4);

            var animation = new AnimateTransformBuilder(Context).MoveByLocal(0, Config, new Vector3(1, 2, 3)).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Vector3(2, 4, 4).IsSimilar(Context.transform.localPosition));
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
