using UnityEngine;
using static MoonriseGames.Toolbox.Types.SmoothingDuration;

namespace MoonriseGames.Toolbox.Types
{
    public class SmoothVector2 : SmoothValue<Vector2>
    {
        public SmoothVector2()
            : base(default, Average, null) { }

        public SmoothVector2(Vector2 initialValue = default, SmoothingDuration smoothingDuration = Average, float? maxVelocity = null)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        public SmoothVector2(Vector2 initialValue, float smoothingDuration, float? maxVelocity)
            : base(initialValue, smoothingDuration, maxVelocity) { }

        protected override Vector2 GetNextValue(Vector2 target)
        {
            var velocity = Velocity;
            var value = Vector2.SmoothDamp(Value, target, ref velocity, SmoothingDuration, MaxVelocity);

            Velocity = velocity;
            return value;
        }

        public static implicit operator Vector2(SmoothVector2 smoothVector2) => smoothVector2.Value;
    }
}
