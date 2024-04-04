using MoonriseGames.Toolbox.Audio;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Audio
{
    public class AudioSourceFactoryTest
    {
        [Test]
        public void ShouldCreateNewAudioSource()
        {
            var source = AudioSourceFactory.GetAudioSource();
            Assert.True(source);
        }

        [Test]
        public void ShouldSetReasonableDefaultValues()
        {
            var source = AudioSourceFactory.GetAudioSource();
            Assert.False(source.playOnAwake);
            Assert.AreEqual(source.rolloffMode, AudioRolloffMode.Linear);
        }
    }
}
