using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using MoonriseGames.Toolbox.Validation;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioEffectTest
    {
        private Mock<IAudioService> Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new Mock<IAudioService>();
            Service.Object.SetAsSingletonInstance();
        }

        [Test]
        public void ShouldBeInvalidWithNoAudioClips()
        {
            var sut = ScriptableObject.CreateInstance<AudioRamp>();
            Assert.Throws<ValidationException>(() => sut.Validate());
        }

        [Test]
        public void ShouldBeValidWithAudioClips()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.SetNonPublicField(new[] { clip });
            sut.Validate();
        }

        [Test]
        public void ShouldPlayAudioClip()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();
            sut.SetNonPublicField(new[] { clip });
            sut.Play();

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip));
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Never());
        }

        [Test]
        public void ShouldPlayAudioClipOneShot()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();
            sut.SetNonPublicField(new[] { clip });
            sut.PlayOneShot();

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Never());
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip));
        }

        [Test]
        public void ShouldNotPlayWithoutAudioClip()
        {
            var sut = ScriptableObject.CreateInstance<AudioEffect>();
            sut.PlayOneShot();

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), null), Times.Never());
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), null), Times.Never());
        }

        [Test]
        public void ShouldStopPlayback()
        {
            var sut = ScriptableObject.CreateInstance<AudioEffect>();
            sut.Stop();

            Service.Verify(x => x.Stop(sut, It.IsAny<AudioPlaybackLocation>()));
        }

        [Test]
        public void ShouldStopPlaybackAtProvidedLocation()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.Stop(location);

            Service.Verify(x => x.Stop(sut, location));
        }

        [Test]
        public void ShouldNotPlayIfFizzle()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetNonPublicField("_fizzleProbability", 1);
            sut.PlayOneShot();
            sut.Play();

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Never());
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Never());
        }

        [Test]
        public void ShouldNotPlayIfDebounce()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetNonPublicField("_debounceDuration", 1);
            sut.PlayOneShot();
            sut.Play();

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Never());
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip), Times.Once());
        }

        [Test]
        public void ShouldNotPlayTheSameRandomClipInARow()
        {
            var clip01 = AudioFactory.GetAudioClip();
            var clip02 = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip01, clip02 });

            for (var i = 0; i < 30; i++)
                sut.PlayOneShot();

            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip01), Times.Exactly(15));
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip02), Times.Exactly(15));
        }

        [Test]
        public void ShouldProvideMaximumDuration()
        {
            var clip01 = AudioFactory.GetAudioClip(1);
            var clip02 = AudioFactory.GetAudioClip(2);
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip01, clip02 });

            Assert.True(clip02.length.IsSimilar(sut.MaxDuration));
        }

        [Test]
        public void ShouldPlayAudioClipAtIndex()
        {
            var clip01 = AudioFactory.GetAudioClip();
            var clip02 = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip01, clip02 });
            sut.Play(audioClipIndex: 1);
            sut.PlayOneShot(audioClipIndex: 1);

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip02));
            Service.Verify(x => x.Play(It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip02));
        }

        [Test]
        public void ShouldUseDefinedValues()
        {
            var clip = AudioFactory.GetAudioClip();
            var group = new Mock<AudioMixerGroup>();
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetNonPublicField("_audioMixerGroup", group.Object);
            sut.SetNonPublicField("_volume", 0.5f);
            sut.SetNonPublicField("_isPriority", true);
            sut.PlayOneShot();

            var defaultPriority = new AudioSourceConfig().Priority;

            Service.Verify(x =>
                x.Play(
                    It.IsAny<AudioPlaybackLocation>(),
                    It.Is<AudioSourceConfig>(x => x.Volume.IsSimilar(0.5f) && x.Priority < defaultPriority && x.Group == group.Object),
                    clip
                )
            );
        }

        [Test]
        public void ShouldUseProvidedLocation()
        {
            var clip = AudioFactory.GetAudioClip();
            var location = new AudioPlaybackLocation(Vector3.one);
            var sut = ScriptableObject.CreateInstance<AudioEffect>();

            sut.SetNonPublicField(new[] { clip });
            sut.Play(location: location);

            Service.Verify(x => x.Play(sut, location, It.IsAny<AudioSourceConfig>(), clip));
        }

        [Test]
        public void ShouldMergeConfigurationValues()
        {
            var clip = AudioFactory.GetAudioClip();
            var group = new Mock<AudioMixerGroup>();

            var config = new AudioSourceConfig(volume: 0.5f, pitch: 0, group: group.Object, priority: 100);
            var sut = ScriptableObject.CreateInstance<AudioEffect>();
            sut.SetNonPublicField(new[] { clip });
            sut.Play(config: config);

            Service.Verify(x =>
                x.Play(
                    sut,
                    It.IsAny<AudioPlaybackLocation>(),
                    It.Is<AudioSourceConfig>(x =>
                        x.Volume.IsSimilar(0.5f) && x.Pitch.IsSimilar(0) && x.Priority == 100 && x.Group == group.Object
                    ),
                    clip
                )
            );
        }
    }
}
