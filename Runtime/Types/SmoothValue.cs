using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Types
{
    public abstract class SmoothValue<T>
    {
        public const float SMOOTHING_DURATION_LONG = .5f;
        public const float SMOOTHING_DURATION_AVERAGE = .1f;
        public const float SMOOTHING_DURATION_SHORT = .065f;

        protected float SmoothingDuration { get; }
        protected float MaxVelocity { get; }

        public T Value { get; set; }
        public T Velocity { get; set; }

        protected SmoothValue(T initialValue, float smoothingDuration, float? maxVelocity = null)
        {
            SmoothingDuration = smoothingDuration;
            MaxVelocity = maxVelocity ?? float.MaxValue;
            Value = initialValue;
        }

        protected SmoothValue(T initialValue, SmoothingDuration smoothingDuration, float? maxVelocity)
            : this(initialValue, 0f, maxVelocity)
        {
            SmoothingDuration = GetSmoothingDuration(smoothingDuration);
        }

        private float GetSmoothingDuration(SmoothingDuration duration) =>
            duration switch
            {
                Types.SmoothingDuration.Average => SMOOTHING_DURATION_AVERAGE,
                Types.SmoothingDuration.Long => SMOOTHING_DURATION_LONG,
                Types.SmoothingDuration.Short => SMOOTHING_DURATION_SHORT,
                _ => throw new ArgumentOutOfRangeException(nameof(duration), duration, null)
            };

        public void Reset()
        {
            Value = default;
            Velocity = default;
        }

        public T Advance(T current, T target)
        {
            Value = current;
            return Advance(target);
        }

        public T Advance(T target)
        {
            Value = GetNextValue(target);
            return Value;
        }

        protected abstract T GetNextValue(T target);
    }
}
