using UnityEngine;

namespace MoonriseGames.Toolbox.Utilities
{
    public static class Vector2Utility
    {
        public static Vector2? GetIntersection(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
        {
            var dir1 = b1 - a1;
            var dir2 = b2 - a2;

            var det = dir1.x * dir2.y - dir1.y * dir2.x;

            if (det == 0)
                return null;

            var t1 = ((a1.x - a2.x) * (a2.y - b2.y) - (a1.y - a2.y) * (a2.x - b2.x)) / det;
            var t2 = -((a1.x - b1.x) * (a1.y - a2.y) - (a1.y - b1.y) * (a1.x - a2.x)) / det;

            if (t1 is < 0 or > 1 || t2 is < 0 or > 1)
                return null;

            return new Vector2(a1.x + t1 * dir1.x, a1.y + t1 * dir1.y);
        }
    }
}
