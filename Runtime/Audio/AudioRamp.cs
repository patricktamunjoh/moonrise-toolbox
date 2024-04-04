using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using MoonriseGames.Toolbox.Validation;
using UnityEngine;
using UnityEngine.Audio;

namespace MoonriseGames.Toolbox.Audio
{
    /// <summary>Object to define and play <see cref="AudioClip"/> with changing volume and pitch based on a set intensity.</summary>
    [CreateAssetMenu(fileName = "Audio Ramp", menuName = "Toolbox/Audio/Audio Ramp", order = Orders.CREATE + 1)]
    public class AudioRamp : ScriptableObject, IValidateable
    {
        [SerializeField]
        private AudioMixerGroup _audioMixerGroup;

        [SerializeField, IntervalRange(0, 1), Space]
        private Interval _volume = new(0, 1);

        [SerializeField, IntervalRange(-3, 3)]
        private Interval _pitch = new(1, 1);

        [SerializeField, Space]
        private AnimationCurve _intensityToVolume = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField, Space]
        private bool _isPriority;

        [SerializeField, Space]
        private AudioClip[] _audioClips;

        public void SetIntensity(float intensity, AudioPlaybackLocation location = null, AudioSourceConfig config = null)
        {
            var clip = _audioClips?.Sample();

            if (clip == null)
                return;

            location ??= new AudioPlaybackLocation();
            config = IncorporateSettingsIntoConfig(Mathf.Clamp01(intensity), config);

            AudioService.Unit.Play(this, location, config, clip);
        }

        public void Stop(AudioPlaybackLocation location = null)
        {
            location ??= new AudioPlaybackLocation();
            AudioService.Unit.Stop(this, location);
        }

        private AudioSourceConfig IncorporateSettingsIntoConfig(float intensity, AudioSourceConfig config)
        {
            config ??= new AudioSourceConfig();

            return config.Copy(
                group: config.Group ? config.Group : _audioMixerGroup,
                priority: config.Priority / (_isPriority ? 2 : 1),
                volume: config.Volume * _volume.Lerp(_intensityToVolume.Evaluate(intensity)),
                pitch: config.Pitch - 1 + _pitch.Lerp(intensity),
                isLooping: true
            );
        }

        public void Validate()
        {
            if (_audioClips == null || _audioClips.Length == 0)
                throw new ValidationException("Missing audio clips");
        }
    }
}
