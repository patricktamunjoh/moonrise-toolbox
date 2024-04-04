using System.Linq;
using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities;
using MoonriseGames.Toolbox.Tests.Utilities.Functions;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioServiceTest
    {
        [SetUp]
        public void Setup() => Function.ClearScene();

        [Test]
        public void ShouldPlayAudioClipWithId()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.NotNull(source);
            Assert.True(source.isPlaying);
            Assert.AreEqual(clip, source.clip);
        }

        [Test]
        public void ShouldPlayAudioClipWithoutId()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.NotNull(source);
            Assert.True(source.isPlaying);
        }

        [Test]
        public void ShouldApplyConfigForAudioClipWithId()
        {
            var config = new AudioSourceConfig(volume: 0.5f, pitch: -1, spread: 0.5f);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), config, clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.True(source.volume.IsSimilar(0.5f));
            Assert.True(source.pitch.IsSimilar(-1));
            Assert.True(source.spread.IsSimilar(0.5f));
        }

        [Test]
        public void ShouldApplyConfigForAudioClipWithoutId()
        {
            var config = new AudioSourceConfig(pitch: -1, spread: 0.5f);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(new AudioPlaybackLocation(), config, clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.True(source.pitch.IsSimilar(-1));
            Assert.True(source.spread.IsSimilar(0.5f));
        }

        [Test]
        public void ShouldIncorporateLocationForAudioClipWithId()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", location, new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.AreEqual(source.spatialBlend, 1);
        }

        [Test]
        public void ShouldIncorporateLocationForAudioClipWithoutId()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(location, new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.AreEqual(source.spatialBlend, 1);
        }

        [Test]
        public void ShouldUpdateConfigForClipWithId()
        {
            var config = new AudioSourceConfig(volume: 0.5f, pitch: -1, spread: 12);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("id", new AudioPlaybackLocation(), config, clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.True(source.volume.IsSimilar(0.5f));
            Assert.True(source.pitch.IsSimilar(-1));
            Assert.True(source.spread.IsSimilar(12));
        }

        [Test]
        public void ShouldApplyLocationForAudioClipWithId()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", location, new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.AreEqual(Vector3.one, source.transform.position);
        }

        [Test]
        public void ShouldApplyLocationForAudioClipWithoutId()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(location, new AudioSourceConfig(), clip);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.AreEqual(Vector3.one, source.transform.position);
        }

        [Test]
        public void ShouldPlayAudioClipWithIdOnlyOnce()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), null);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.True(source.isPlaying);
            Assert.AreEqual(clip, source.clip);
        }

        [Test]
        public void ShouldReuseAudioSourceThatIsNotPlaying()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), null);
            var source = Object.FindObjectOfType<AudioSource>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);

            Assert.True(source.isPlaying);
            Assert.AreEqual(clip, source.clip);
        }

        [Test]
        public void ShouldReuseAudioSourceForOneShotWithSameConfigAndLocation()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(), clip);

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.AreEqual(1, sources.Length);
        }

        [Test]
        public void ShouldReuseAudioSourceForOneShotWithDifferentVolume()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(volume: 0.8f), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(volume: 0.2f), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(volume: 0.1f), clip);

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.AreEqual(1, sources.Length);
        }

        [Test]
        public void ShouldNotReuseAudioSourceForOneShotWithDifferentConfigOrLocation()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(pitch: 0), clip);
            sut.Play(new AudioPlaybackLocation(Vector3.one), new AudioSourceConfig(), clip);

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.AreEqual(3, sources.Length);
        }

        [Test]
        public void ShouldStopForIdAndLocation()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", location, new AudioSourceConfig(), clip);
            sut.Stop("id", location);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.False(source.isPlaying);
        }

        [Test]
        public void ShouldOnlyStopIfIdAndLocationMatch()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("other id", location, new AudioSourceConfig(), clip);
            sut.Stop("id", location);

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.True(sources.All(x => x.isPlaying));
        }

        [Test]
        public void ShouldStopAllWithId()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("id", location, new AudioSourceConfig(), clip);
            sut.Stop("id");

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.True(sources.All(x => !x.isPlaying));
        }

        [Test]
        public void ShouldOnlyStopIfIdMatches()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("other id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("different id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Stop("id");

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.True(sources.All(x => x.isPlaying));
        }

        [Test]
        public void ShouldStopAllWithClip()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Play("other id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip);
            sut.Stop(clip);

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.True(sources.All(x => !x.isPlaying));
        }

        [Test]
        public void ShouldOnlyStopIfClipMatches()
        {
            var clip01 = AudioFactory.GetAudioClip(10);
            var clip02 = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(), new AudioSourceConfig(), clip02);
            sut.Stop(clip01);

            var source = Object.FindObjectOfType<AudioSource>();

            Assert.True(source.isPlaying);
        }

        [Test]
        public void ShouldClearAllAudioSources()
        {
            var clip = AudioFactory.GetAudioClip(10);
            var sut = new GameObject().AddComponent<AudioService>();

            sut.Play("id", new AudioPlaybackLocation(Vector3.down), new AudioSourceConfig(), clip);
            sut.Play(new AudioPlaybackLocation(), new AudioSourceConfig(pitch: 0), clip);
            sut.Clear();

            var sources = Object.FindObjectsOfType<AudioSource>();

            Assert.Zero(sources.Length);
        }
    }
}
