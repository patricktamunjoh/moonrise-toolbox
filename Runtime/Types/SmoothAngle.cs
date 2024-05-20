using UnityEngine;
using static MoonriseGames.Toolbox.Types.SmoothingDuration;

namespace MoonriseGames.Toolbox.Types
{
    public class SmoothAngle : SmoothValue<float>
    {
        public SmoothAngle()
            : base(default, Average, null) { }

        public SmoothAngle(float initialValue = default, SmoothingDuration smoothingDuration = Average, float? maxVelocity = null)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        public SmoothAngle(float initialValue, float smoothingDuration, float? maxVelocity)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        protected override float GetNextValue(float target)
        {
            var velocity = Velocity;
            var value = Mathf.SmoothDampAngle(Value, target, ref velocity, SmoothingDuration, MaxVelocity);

            Velocity = velocity;
            return value;
        }

        public static implicit operator float(SmoothAngle smoothAngle) => smoothAngle.Value;
    }
}
