using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    internal static class AudioSourceFactory
    {
        public static AudioSource GetAudioSource()
        {
            var go = new GameObject("Audio Source");
            var source = go.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.rolloffMode = AudioRolloffMode.Linear;

            return source;
        }
    }
}
