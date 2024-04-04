using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Tests.Utilities;
using MoonriseGames.Toolbox.Tests.Utilities.Extensions;
using MoonriseGames.Toolbox.Types;
using MoonriseGames.Toolbox.Validation;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioRampTest
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
        public void ShouldPlayAudioClipWhenSettingIntensity()
        {
            var clip = AudioFactory.GetAudioClip();
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetIntensity(1);

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), clip));
        }

        [Test]
        public void ShouldNotPlayWithoutAudioClip()
        {
            var sut = ScriptableObject.CreateInstance<AudioRamp>();
            sut.SetIntensity(1);

            Service.Verify(x => x.Play(sut, It.IsAny<AudioPlaybackLocation>(), It.IsAny<AudioSourceConfig>(), null), Times.Never());
        }

        [Test]
        public void ShouldStopPlayback()
        {
            var sut = ScriptableObject.CreateInstance<AudioRamp>();
            sut.Stop();

            Service.Verify(x => x.Stop(sut, It.IsAny<AudioPlaybackLocation>()));
        }

        [Test]
        public void ShouldStopPlaybackAtProvidedLocation()
        {
            var location = new AudioPlaybackLocation(Vector3.one);
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.Stop(location);

            Service.Verify(x => x.Stop(sut, location));
        }

        [Test]
        public void ShouldUseDefinedValues()
        {
            var clip = AudioFactory.GetAudioClip();
            var group = new Mock<AudioMixerGroup>();
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetNonPublicField("_audioMixerGroup", group.Object);
            sut.SetNonPublicField("_volume", new Interval(0.25f, 0.75f));
            sut.SetNonPublicField("_pitch", new Interval(0.25f, 0.75f));
            sut.SetNonPublicField("_intensityToVolume", AnimationCurve.Linear(0, 0.5f, 1, 0.5f));
            sut.SetNonPublicField("_isPriority", true);
            sut.SetIntensity(0.5f);

            var defaultPriority = new AudioSourceConfig().Priority;

            Service.Verify(x =>
                x.Play(
                    sut,
                    It.IsAny<AudioPlaybackLocation>(),
                    It.Is<AudioSourceConfig>(x =>
                        x.Volume.IsSimilar(0.5f) && x.Pitch.IsSimilar(0.5f) && x.Priority < defaultPriority && x.Group == group.Object
                    ),
                    clip
                )
            );
        }

        [Test]
        public void ShouldUseProvidedLocation()
        {
            var clip = AudioFactory.GetAudioClip();
            var location = new AudioPlaybackLocation(Vector3.one);
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetIntensity(1, location: location);

            Service.Verify(x => x.Play(sut, location, It.IsAny<AudioSourceConfig>(), clip));
        }

        [Test]
        public void ShouldMergeConfigurationValues()
        {
            var clip = AudioFactory.GetAudioClip();
            var group = new Mock<AudioMixerGroup>();
            var config = new AudioSourceConfig(volume: 0.5f, pitch: 0, group: group.Object, priority: 100);
            var sut = ScriptableObject.CreateInstance<AudioRamp>();

            sut.SetNonPublicField(new[] { clip });
            sut.SetIntensity(1, config: config);

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
