using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    public class AudioPlaybackLocation
    {
        private static Transform GlobalParent { get; set; }

        public Transform Parent { get; }
        public Vector3? Point { get; }

        public bool IsGlobal => !Parent && !Point.HasValue;

        public AudioPlaybackLocation() { }

        public AudioPlaybackLocation(Transform parent) => Parent = parent;

        public AudioPlaybackLocation(Vector3 point) => Point = point;

        internal void ApplyToAudioSource(AudioSource source)
        {
            source.transform.parent = !Parent ? GetGlobalParent() : Parent;
            source.transform.localPosition = Point ?? Vector3.zero;
        }

        private Transform GetGlobalParent()
        {
            if (!GlobalParent)
                GlobalParent = new GameObject("Audio Sources").transform;
            return GlobalParent;
        }

        public override bool Equals(object obj)
        {
            if (obj is not AudioPlaybackLocation location)
                return false;

            return Parent == location.Parent && Point == location.Point;
        }

        public override int GetHashCode() => HashCode.Combine(Parent, Point);
    }
}
