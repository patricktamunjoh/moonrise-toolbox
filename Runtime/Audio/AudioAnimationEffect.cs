using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Audio
{
    [Serializable]
    internal class AudioAnimationEffect
    {
        [SerializeField]
        private string _name;
        public string Name => _name;

        [SerializeField]
        private AudioEffect _audioEffect;
        public AudioEffect AudioEffect => _audioEffect;
    }
}
