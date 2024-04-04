using MoonriseGames.Toolbox.Animations;
using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class AnimateExtensionsTest
    {
        [Test]
        public void ShouldProvideBuilderForTransformAndContext()
        {
            var transform = new GameObject().transform;
            var context = new GameObject().AddComponent<SampleBehaviour>();

            transform.Animate(context).MoveTo(0, new AnimationConfigBuilder().Build(), Vector3.one).Build().Start();

            Assert.True(Vector3.one.IsSimilar(transform.position));
            Assert.True(Vector3.zero.IsSimilar(context.transform.position));
        }

        [Test]
        public void ShouldUseProvidedContextForAnimatingTransform()
        {
            var transform = new GameObject().transform;

            transform.Animate(null).MoveTo(0, new AnimationConfigBuilder().Build(), Vector3.one).Build().Start();

            Assert.True(Vector3.zero.IsSimilar(transform.position));
        }

        [Test]
        public void ShouldProvideBuilderForMonoBehaviour()
        {
            var context = new GameObject().AddComponent<SampleBehaviour>();

            context.Animate().MoveTo(0, new AnimationConfigBuilder().Build(), Vector3.one).Build().Start();

            Assert.True(Vector3.one.IsSimilar(context.transform.position));
        }

        [Test]
        public void ShouldProvideBuilderForGraphicAndContext()
        {
            var graphic = new GameObject().AddComponent<Image>();
            var context = new GameObject().AddComponent<SampleBehaviour>();

            graphic.Animate(context).FadeTo(0, new AnimationConfigBuilder().Build(), 0.5f).Build().Start();

            Assert.True(0.5f.IsSimilar(graphic.color.a));
        }

        [Test]
        public void ShouldUseProvidedContextForAnimatingGraphic()
        {
            var graphic = new GameObject().AddComponent<Image>();

            graphic.color = Color.white;
            graphic.Animate(null).FadeTo(0, new AnimationConfigBuilder().Build(), 0.5f).Build().Start();

            Assert.True(1f.IsSimilar(graphic.color.a));
        }

        [Test]
        public void ShouldProvideBuilderForGraphic()
        {
            var graphic = new GameObject().AddComponent<Image>();

            graphic.Animate().FadeTo(0, new AnimationConfigBuilder().Build(), 0.5f).Build().Start();

            Assert.True(0.5f.IsSimilar(graphic.color.a));
        }

        private class SampleBehaviour : MonoBehaviour { }
    }
}
