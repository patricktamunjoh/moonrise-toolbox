using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    public interface IAudioService
    {
        void Play(object effectId, AudioPlaybackLocation location, AudioSourceConfig config, AudioClip clip);

        void Play(AudioPlaybackLocation location, AudioSourceConfig config, AudioClip clip);

        void Stop(object effectId, AudioPlaybackLocation location);

        void Stop(object effectId);

        void Stop(AudioClip clip);

        void Clear();
    }
}
