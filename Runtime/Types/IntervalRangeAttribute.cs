using UnityEngine;

namespace MoonriseGames.Toolbox.Types
{
    public class IntervalRangeAttribute : PropertyAttribute
    {
        public float Min { get; }
        public float Max { get; }

        public IntervalRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
