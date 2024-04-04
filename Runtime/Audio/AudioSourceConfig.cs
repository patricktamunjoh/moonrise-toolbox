using System;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace MoonriseGames.Toolbox.Audio
{
    //TODO: More audio source settings could be added for greater flexibility
    public class AudioSourceConfig
    {
        public AudioMixerGroup Group { get; }

        public float MaxDistance { get; }
        public float SpatialBlend { get; }
        public float Spread { get; }
        public float Volume { get; }
        public float Pitch { get; }
        public int Priority { get; }
        public bool IsLooping { get; }

        public AudioSourceConfig(
            AudioMixerGroup group = null,
            float? maxDistance = null,
            float? spatialBlend = null,
            float? spread = null,
            float? volume = null,
            float? pitch = null,
            int? priority = null,
            bool? isLooping = null
        )
        {
            Group = group;
            MaxDistance = maxDistance ?? 50;
            SpatialBlend = spatialBlend ?? 0;
            Spread = spread ?? 100;
            Volume = volume ?? 1;
            Pitch = pitch ?? 1;
            Priority = priority ?? 128;
            IsLooping = isLooping ?? false;
        }

        internal AudioSourceConfig Copy(
            AudioMixerGroup group = null,
            float? maxDistance = null,
            float? spatialBlend = null,
            float? spread = null,
            float? volume = null,
            float? pitch = null,
            int? priority = null,
            bool? isLooping = null
        ) =>
            new(
                group: group ? group : Group,
                maxDistance: maxDistance ?? MaxDistance,
                spatialBlend: spatialBlend ?? SpatialBlend,
                spread: spread ?? Spread,
                volume: volume ?? Volume,
                pitch: pitch ?? Pitch,
                priority: priority ?? Priority,
                isLooping: isLooping ?? IsLooping
            );

        internal AudioSourceConfig Incorporate(AudioPlaybackLocation location)
        {
            if (location.IsGlobal)
                return Copy(spatialBlend: 0);

            return Copy(spatialBlend: SpatialBlend > 0 ? SpatialBlend : 1);
        }

        internal void ApplyToAudioSource(AudioSource source)
        {
            source.outputAudioMixerGroup = Group;

            source.maxDistance = MaxDistance;
            source.spatialBlend = SpatialBlend;
            source.spread = Spread;
            source.volume = Volume;
            source.pitch = Pitch;
            source.priority = Priority;
            source.loop = IsLooping;
        }

        public override bool Equals(object obj)
        {
            if (obj is not AudioSourceConfig config)
                return false;

            return Group == config.Group
                && MaxDistance.IsSimilar(config.MaxDistance)
                && SpatialBlend.IsSimilar(config.SpatialBlend)
                && Spread.IsSimilar(config.Spread)
                && Volume.IsSimilar(config.Volume)
                && Pitch.IsSimilar(config.Pitch)
                && Priority == config.Priority
                && IsLooping == config.IsLooping;
        }

        public override int GetHashCode() => HashCode.Combine(Group, MaxDistance, SpatialBlend, Spread, Volume, Pitch, Priority, IsLooping);
    }
}
