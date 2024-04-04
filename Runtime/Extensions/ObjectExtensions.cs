using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object target) => target == null || target is Object obj && !obj;
    }
}
