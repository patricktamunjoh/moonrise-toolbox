using System;
using System.Linq;
using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    /// <summary>Allows playing <see cref="AudioEffect"/> by name through animation events.</summary>
    [DisallowMultipleComponent]
    public class AudioAnimationEvent : MonoBehaviour
    {
        [SerializeField]
        private AudioAnimationEffect[] _effects;

        public void Play(string name)
        {
            var audioEffect = GetAudioEffect(name);

            if (audioEffect == null)
                return;

            audioEffect.PlayOneShot();
        }

        public void PlayAtTransform(string name)
        {
            var audioEffect = GetAudioEffect(name);

            if (audioEffect == null)
                return;

            audioEffect.PlayOneShot(location: new AudioPlaybackLocation(transform));
        }

        private AudioEffect GetAudioEffect(string name) =>
            _effects.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase))?.AudioEffect;
    }
}
