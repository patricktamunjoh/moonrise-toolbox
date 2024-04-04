using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class TimeExtensions
    {
        public static bool HasElapsed(this float duration, float timeLastEvent, bool useUnscaledTime = false) =>
            (useUnscaledTime ? Time.unscaledTime : Time.time) - timeLastEvent >= duration;
    }
}
