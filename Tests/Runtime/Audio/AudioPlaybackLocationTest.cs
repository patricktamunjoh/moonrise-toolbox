using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioPlaybackLocationTest
    {
        [SetUp]
        public void Setup() => Function.ClearScene();

        [Test]
        public void ShouldBeGlobalByDefault()
        {
            var sut = new AudioPlaybackLocation();
            Assert.True(sut.IsGlobal);
        }

        [Test]
        public void ShouldBeEqualWithSameParentOrPoint()
        {
            var sutGlobal01 = new AudioPlaybackLocation();
            var sutGlobal02 = new AudioPlaybackLocation();

            var sutPoint01 = new AudioPlaybackLocation(Vector3.one);
            var sutPoint02 = new AudioPlaybackLocation(Vector3.one);

            var transform = new GameObject().transform;
            var sutTransform01 = new AudioPlaybackLocation(transform);
            var sutTransform02 = new AudioPlaybackLocation(transform);

            Assert.AreEqual(sutGlobal01, sutGlobal02);
            Assert.AreEqual(sutPoint01, sutPoint02);
            Assert.AreEqual(sutTransform01, sutTransform02);

            Assert.AreNotEqual(sutTransform01, sutGlobal01);
            Assert.AreNotEqual(sutPoint02, sutGlobal01);
            Assert.AreNotEqual(sutPoint02, sutTransform02);
        }

        [Test]
        public void ShouldApplyTransformToSource()
        {
            var transform = new GameObject().transform;
            var source = AudioSourceFactory.GetAudioSource();
            var sut = new AudioPlaybackLocation(transform);

            sut.ApplyToAudioSource(source);

            Assert.AreEqual(transform, source.transform.parent);
        }

        [Test]
        public void ShouldApplyPointToSource()
        {
            var source = AudioSourceFactory.GetAudioSource();
            var sut = new AudioPlaybackLocation(Vector3.one);

            sut.ApplyToAudioSource(source);

            Assert.AreEqual(Vector3.one, source.transform.position);
        }

        [Test]
        public void ShouldApplyGlobalParentToSource()
        {
            var source01 = AudioSourceFactory.GetAudioSource();
            var source02 = AudioSourceFactory.GetAudioSource();

            var sut01 = new AudioPlaybackLocation(Vector3.one);
            var sut02 = new AudioPlaybackLocation();

            sut01.ApplyToAudioSource(source01);
            sut02.ApplyToAudioSource(source02);

            Assert.AreEqual(source01.transform.parent, source02.transform.parent);
            Assert.NotNull(source01.transform.parent);
        }
    }
}
