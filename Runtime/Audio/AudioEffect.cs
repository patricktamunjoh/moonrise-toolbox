using System;
using System.Linq;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Validation;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace MoonriseGames.Toolbox.Audio
{
    /// <summary>Object to define and play <see cref="AudioClip"/> through the <see cref="AudioService"/>.</summary>
    [CreateAssetMenu(fileName = "Audio Effect", menuName = "Toolbox/Audio/Audio Effect", order = Orders.CREATE)]
    public class AudioEffect : ScriptableObject, IValidateable
    {
        [SerializeField]
        private AudioMixerGroup _audioMixerGroup;

        [SerializeField, Range(0, 1), Space]
        private float _volume = 1;

        [SerializeField, Range(0, 1)]
        private float _fizzleProbability;

        [SerializeField]
        private float _debounceDuration;

        //Whether the audio effect should have increased priority
        //This also means that it blocks a playback slot even when not audible
        [SerializeField, Space]
        private bool _isPriority;

        [SerializeField, Space]
        private AudioClip[] _audioClips;

        [NonSerialized]
        private int _indexLastRandomClip;

        [NonSerialized]
        private float _timeLastPlayback = float.MinValue;

        public float Volume => _volume;
        public float MaxDuration => _audioClips.Max(x => x.length);

        private float CurrentTime => Time.unscaledTime;

        public void Play(AudioPlaybackLocation location = null, AudioSourceConfig config = null, int? audioClipIndex = null) =>
            Play(location, config, audioClipIndex, false);

        public void PlayOneShot(AudioPlaybackLocation location = null, AudioSourceConfig config = null, int? audioClipIndex = null) =>
            Play(location, config, audioClipIndex, true);

        private void Play(AudioPlaybackLocation location, AudioSourceConfig config, int? audioClipIndex, bool doPlayOneShot)
        {
            if (!IsPlaybackPossible())
                return;

            var clip = GetNextAudioClip(audioClipIndex);

            if (clip == null)
                return;

            location ??= new AudioPlaybackLocation();
            config = IncorporateSettingsIntoConfig(config);

            if (doPlayOneShot)
                AudioService.Unit.Play(location, config, clip);
            else
                AudioService.Unit.Play(this, location, config, clip);

            _timeLastPlayback = CurrentTime;
        }

        public void Stop(AudioPlaybackLocation location = null)
        {
            location ??= new AudioPlaybackLocation();
            AudioService.Unit.Stop(this, location);
        }

        private bool IsPlaybackPossible() => _fizzleProbability.Check().Not() && _debounceDuration.HasElapsed(_timeLastPlayback, true);

        private AudioClip GetNextAudioClip(int? index)
        {
            if (_audioClips == null || _audioClips.Length == 0)
                return null;

            return index == null ? GetRandomAudioClip() : _audioClips[index.Value % _audioClips.Length];
        }

        private AudioClip GetRandomAudioClip()
        {
            var index = Random.Range(0, _audioClips.Length);

            if (index == _indexLastRandomClip)
                index = (index + 1) % _audioClips.Length;

            _indexLastRandomClip = index;
            return _audioClips[index];
        }

        private AudioSourceConfig IncorporateSettingsIntoConfig(AudioSourceConfig config)
        {
            config ??= new AudioSourceConfig();

            return config.Copy(
                group: config.Group ? config.Group : _audioMixerGroup,
                volume: config.Volume * Volume,
                priority: config.Priority / (_isPriority ? 2 : 1)
            );
        }

        public void Validate()
        {
            if (_audioClips == null || _audioClips.Length == 0)
                throw new ValidationException("Missing audio clips");
        }
    }
}
