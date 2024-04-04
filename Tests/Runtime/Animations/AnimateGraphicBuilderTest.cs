using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace MoonriseGames.Toolbox.Tests.Animations
{
    public class AnimateGraphicBuilderTest
    {
        private Image Graphic { get; set; }
        private AnimationConfig Config { get; } = new AnimationConfigBuilder().Build();

        [SetUp]
        public void Setup()
        {
            Function.ClearScene();
            Graphic = new GameObject().AddComponent<Image>();
        }

        [Test]
        public void ShouldUseProvidedContext()
        {
            Graphic.color = new Color(0.5f, 0.2f, 0.7f, 0.3f);

            var context = new GameObject().AddComponent<SampleBehaviour>();
            Object.DestroyImmediate(context);

            new AnimateGraphicBuilder(Graphic, context).FadeTo(0, Config, 0).Build().Start();

            Assert.True(new Color(0.5f, 0.2f, 0.7f, 0.3f).IsSimilar(Graphic.color));
        }

        [Test]
        public void ShouldFadeToAlpha()
        {
            Graphic.color = new Color(0.5f, 0.2f, 0.7f, 0.3f);

            new AnimateGraphicBuilder(Graphic).FadeTo(1, Config, 1).Build().Start();
            Assert.True(new Color(0.5f, 0.2f, 0.7f, 0.3f).IsSimilar(Graphic.color));

            new AnimateGraphicBuilder(Graphic).FadeTo(0, Config, 1).Build().Start();
            Assert.True(new Color(0.5f, 0.2f, 0.7f, 1f).IsSimilar(Graphic.color));
        }

        [Test]
        public void ShouldFadeBetweenAlpha()
        {
            Graphic.color = new Color(0.5f, 0.2f, 0.7f, 0.3f);

            new AnimateGraphicBuilder(Graphic).Fade(1, Config, 1, 0).Build().Start();
            Assert.True(new Color(0.5f, 0.2f, 0.7f, 1).IsSimilar(Graphic.color));

            new AnimateGraphicBuilder(Graphic).Fade(0, Config, 1, 0).Build().Start();
            Assert.True(new Color(0.5f, 0.2f, 0.7f, 0).IsSimilar(Graphic.color));
        }

        [Test]
        public void ShouldResetToOriginalAlpha()
        {
            Graphic.color = new Color(0.5f, 0.2f, 0.7f, 0.3f);

            var animation = new AnimateGraphicBuilder(Graphic).FadeTo(0, Config, 1).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Color(0.5f, 0.2f, 0.7f, 0.3f).IsSimilar(Graphic.color));
        }

        [Test]
        public void ShouldResetToInitialAlpha()
        {
            Graphic.color = new Color(0.5f, 0.2f, 0.7f, 0.3f);

            var animation = new AnimateGraphicBuilder(Graphic).Fade(0, Config, 1, 0).Build();

            animation.Start();
            animation.Reset();

            Assert.True(new Color(0.5f, 0.2f, 0.7f, 1).IsSimilar(Graphic.color));
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
