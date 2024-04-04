using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    public class AudioService : SingletonGlobal<IAudioService>, IAudioService
    {
        private Dictionary<(object effectId, AudioPlaybackLocation location), AudioSource> Sources { get; set; } = new();
        private Dictionary<(AudioPlaybackLocation location, AudioSourceConfig config), AudioSource> SourcesOneShot { get; set; } = new();

        public void Play(object effectId, AudioPlaybackLocation location, AudioSourceConfig config, AudioClip clip)
        {
            var source = GetAudioSource(effectId, location);
            config.Incorporate(location).ApplyToAudioSource(source);

            if (source.isPlaying)
                return;

            location.ApplyToAudioSource(source);

            source.clip = clip;
            source.Play();
        }

        public void Play(AudioPlaybackLocation location, AudioSourceConfig config, AudioClip clip)
        {
            var sourceConfig = config.Copy(volume: 1).Incorporate(location);
            var source = GetAudioSource(location, sourceConfig);

            location.ApplyToAudioSource(source);
            sourceConfig.ApplyToAudioSource(source);

            source.PlayOneShot(clip, config.Volume);
        }

        public void Stop(object effectId, AudioPlaybackLocation location)
        {
            if (Sources.TryGetValue((effectId, location), out var source))
            {
                if (source)
                    source.Stop();
            }
        }

        public void Stop(object effectId)
        {
            foreach (var item in Sources.Where(x => x.Key.effectId == effectId && x.Value))
                item.Value.Stop();
        }

        public void Stop(AudioClip clip)
        {
            foreach (var source in Sources.Values.Where(x => x && x.clip == clip))
                source.Stop();
        }

        public void Clear()
        {
            foreach (var source in Sources.Values.Concat(SourcesOneShot.Values))
                source.DestroyGameObject();

            Sources.Clear();
            SourcesOneShot.Clear();
        }

        private AudioSource GetAudioSource(object effectId, AudioPlaybackLocation location)
        {
            if (Sources.TryGetValue((effectId, location), out var source))
                return source;

            source = GetNewOrUnusedAudioSource();
            return Sources[(effectId, location)] = source;
        }

        private AudioSource GetAudioSource(AudioPlaybackLocation location, AudioSourceConfig config)
        {
            if (SourcesOneShot.TryGetValue((location, config), out var source))
                return source;

            source = GetNewOrUnusedAudioSource();
            return SourcesOneShot[(location, config)] = source;
        }

        private AudioSource GetNewOrUnusedAudioSource()
        {
            CleanupDestroyedAudioSources();
            var source = GetUnusedAudioSource(Sources) ?? GetUnusedAudioSource(SourcesOneShot);
            return source ? source : AudioSourceFactory.GetAudioSource();
        }

        private AudioSource GetUnusedAudioSource<T>(IDictionary<T, AudioSource> sources)
        {
            var item = sources.FirstOrDefault(x => x.Value && !x.Value.isPlaying);

            if (default(KeyValuePair<T, AudioSource>).Equals(item))
                return null;

            sources.Remove(item.Key);
            return item.Value;
        }

        private void CleanupDestroyedAudioSources()
        {
            Sources = Sources.Where(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            SourcesOneShot = SourcesOneShot.Where(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
