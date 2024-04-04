using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Types
{
    [Serializable]
    public class Interval
    {
        [SerializeField]
        private float _start;

        [SerializeField]
        private float _end;

        public float Start
        {
            get => _start;
            set => _start = value;
        }
        public float End
        {
            get => _end;
            set => _end = value;
        }

        public Interval(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public bool IsInside(float value) => value >= Start && value <= End;

        public float Lerp(float t) => Mathf.Lerp(_start, _end, Mathf.Clamp01(t));
    }
}
