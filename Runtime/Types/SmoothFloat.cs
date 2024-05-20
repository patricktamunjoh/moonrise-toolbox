using UnityEngine;
using static MoonriseGames.Toolbox.Types.SmoothingDuration;

namespace MoonriseGames.Toolbox.Types
{
    public class SmoothFloat : SmoothValue<float>
    {
        public SmoothFloat()
            : base(default, Average, null) { }

        public SmoothFloat(float initialValue = default, SmoothingDuration smoothingDuration = Average, float? maxVelocity = null)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        public SmoothFloat(float initialValue, float smoothingDuration, float? maxVelocity)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        protected override float GetNextValue(float target)
        {
            var velocity = Velocity;
            var value = Mathf.SmoothDamp(Value, target, ref velocity, SmoothingDuration, MaxVelocity);

            Velocity = velocity;
            return value;
        }

        public static implicit operator float(SmoothFloat smoothFloat) => smoothFloat.Value;
    }
}
