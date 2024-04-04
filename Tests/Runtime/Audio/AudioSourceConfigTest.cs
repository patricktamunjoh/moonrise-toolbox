using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioSourceConfigTest
    {
        [Test]
        public void ShouldUseReasonableDefaultValues()
        {
            var sut = new AudioSourceConfig();

            Assert.AreEqual(1, sut.Pitch);
            Assert.AreEqual(1, sut.Volume);
            Assert.AreEqual(100, sut.Spread);
            Assert.AreEqual(50, sut.MaxDistance);
            Assert.AreEqual(128, sut.Priority);
            Assert.AreEqual(0, sut.SpatialBlend);
            Assert.AreEqual(false, sut.IsLooping);
        }

        [Test]
        public void ShouldOverwriteValuesOnCopy()
        {
            var sut01 = new AudioSourceConfig(pitch: 12, volume: 0.1f, spread: 2, maxDistance: 2, priority: 2);
            var sut02 = sut01.Copy(pitch: 14, volume: 0, spread: 3, maxDistance: 100, priority: 10, isLooping: true);

            Assert.AreEqual(14, sut02.Pitch);
            Assert.AreEqual(0, sut02.Volume);
            Assert.AreEqual(3, sut02.Spread);
            Assert.AreEqual(100, sut02.MaxDistance);
            Assert.AreEqual(10, sut02.Priority);
            Assert.AreEqual(true, sut02.IsLooping);
        }

        [Test]
        public void ShouldRetainValuesOnCopy()
        {
            var group = new Mock<AudioMixerGroup>().Object;
            var sut01 = new AudioSourceConfig(group: group, pitch: 12, volume: 0.1f, spread: 2, maxDistance: 2, priority: 2);
            var sut02 = sut01.Copy();

            Assert.AreEqual(sut01.Group, sut02.Group);
            Assert.AreEqual(sut01.Priority, sut02.Priority);
            Assert.AreEqual(sut01.Pitch, sut02.Pitch);
            Assert.AreEqual(sut01.Volume, sut02.Volume);
            Assert.AreEqual(sut01.Spread, sut02.Spread);
            Assert.AreEqual(sut01.MaxDistance, sut02.MaxDistance);
        }

        [Test]
        public void ShouldUpdateSpatialBlendWhenIncorporating()
        {
            var locationGlobal = new AudioPlaybackLocation();
            var locationLocal = new AudioPlaybackLocation(new GameObject().transform);

            var sut01 = new AudioSourceConfig(spatialBlend: 0.8f);
            var sut02 = new AudioSourceConfig(spatialBlend: 0);

            Assert.True(sut01.Incorporate(locationGlobal).SpatialBlend.IsSimilar(0));
            Assert.True(sut01.Incorporate(locationLocal).SpatialBlend.IsSimilar(0.8f));
            Assert.True(sut02.Incorporate(locationLocal).SpatialBlend.IsSimilar(1f));
        }

        [Test]
        public void ShouldApplySettingsToAudioSource()
        {
            var group = new Mock<AudioMixerGroup>().Object;
            var source = AudioSourceFactory.GetAudioSource();
            var sut = new AudioSourceConfig(group: group, pitch: 12, volume: 0.1f, spread: 2, maxDistance: 2, priority: 2, isLooping: true);

            sut.ApplyToAudioSource(source);

            Assert.AreEqual(source.outputAudioMixerGroup, group);
            Assert.AreEqual(source.pitch, sut.Pitch);
            Assert.AreEqual(source.volume, sut.Volume);
            Assert.AreEqual(source.spread, sut.Spread);
            Assert.AreEqual(source.maxDistance, sut.MaxDistance);
            Assert.AreEqual(source.priority, sut.Priority);
            Assert.AreEqual(source.loop, sut.IsLooping);
        }

        [Test]
        public void ShouldBeEqualWithSameSettings()
        {
            var sut01 = new AudioSourceConfig(pitch: 12, volume: 7, spread: 10, maxDistance: 100, priority: 2, isLooping: true);
            var sut02 = new AudioSourceConfig(pitch: 12, volume: 7, spread: 10, maxDistance: 100, priority: 2, isLooping: true);

            Assert.AreEqual(sut01, sut02);
        }

        [Test]
        public void ShouldNotBeEqualWithDifferentSettings()
        {
            var sut01 = new AudioSourceConfig(pitch: 12, volume: 7, spread: 10, maxDistance: 100, priority: 2, isLooping: true);
            var sut02 = new AudioSourceConfig();

            Assert.AreNotEqual(sut01, sut02);
        }
    }
}
