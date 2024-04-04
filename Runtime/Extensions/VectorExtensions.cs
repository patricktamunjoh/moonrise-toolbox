using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 MoveTowards(this Vector2 origin, Vector2 target, float maxDistance) =>
            ((Vector3)origin).MoveTowards(target, maxDistance);

        public static Vector3 MoveTowards(this Vector3 origin, Vector3 target, float maxDistance)
        {
            var dir = target - origin;
            return origin + dir.normalized * Mathf.Min(dir.magnitude, maxDistance);
        }

        public static float Sum(this Vector4 vector) => vector.x + vector.y + vector.z + vector.w;

        public static float Sum(this Vector3 vector) => vector.x + vector.y + vector.z;

        public static float Sum(this Vector2 vector) => ((Vector3)vector).Sum();

        public static Vector4 WithX(this Vector4 vector, float x) => new(x, vector.y, vector.z, vector.w);

        public static Vector4 WithY(this Vector4 vector, float y) => new(vector.x, y, vector.z, vector.w);

        public static Vector4 WithZ(this Vector4 vector, float z) => new(vector.x, vector.y, z, vector.w);

        public static Vector4 WithW(this Vector4 vector, float w) => new(vector.x, vector.y, vector.z, w);

        public static Vector3 WithX(this Vector3 vector, float x) => new(x, vector.y, vector.z);

        public static Vector3 WithY(this Vector3 vector, float y) => new(vector.x, y, vector.z);

        public static Vector3 WithZ(this Vector3 vector, float z) => new(vector.x, vector.y, z);

        public static Vector2 WithX(this Vector2 vector, float x) => new(x, vector.y);

        public static Vector2 WithY(this Vector2 vector, float y) => new(vector.x, y);

        public static Vector4 AddX(this Vector4 vector, float x) => new(vector.x + x, vector.y, vector.z, vector.w);

        public static Vector4 AddY(this Vector4 vector, float y) => new(vector.x, vector.y + y, vector.z, vector.w);

        public static Vector4 AddZ(this Vector4 vector, float z) => new(vector.x, vector.y, vector.z + z, vector.w);

        public static Vector4 AddW(this Vector4 vector, float w) => new(vector.x, vector.y, vector.z, vector.w + w);

        public static Vector3 AddX(this Vector3 vector, float x) => new(vector.x + x, vector.y, vector.z);

        public static Vector3 AddY(this Vector3 vector, float y) => new(vector.x, vector.y + y, vector.z);

        public static Vector3 AddZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z + z);

        public static Vector2 AddX(this Vector2 vector, float x) => new(vector.x + x, vector.y);

        public static Vector2 AddY(this Vector2 vector, float y) => new(vector.x, vector.y + y);

        public static bool IsSimilar(this Vector4 vector, Vector4 other) =>
            vector.x.IsSimilar(other.x) && vector.y.IsSimilar(other.y) && vector.z.IsSimilar(other.z) && vector.w.IsSimilar(other.w);

        public static bool IsSimilar(this Vector3 vector, Vector3 other) =>
            vector.x.IsSimilar(other.x) && vector.y.IsSimilar(other.y) && vector.z.IsSimilar(other.z);

        public static bool IsSimilar(this Vector2 vector, Vector2 other) => vector.x.IsSimilar(other.x) && vector.y.IsSimilar(other.y);

        public static float DistanceToLine(this Vector3 vector, Vector3 a, Vector3 b)
        {
            var line = (b - a).normalized;
            var projection = a + Vector3.Dot(vector - a, line) * line;
            return Vector3.Distance(vector, projection);
        }
    }
}
