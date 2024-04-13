using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Paths;
using UnityEngine;

namespace MoonriseGames.Toolbox.Utilities
{
    public static class GizmoDrawingUtility
    {
        private const float ARROW_HEAD_SIZE = 0.4f;
        private const float ARROW_HEAD_TO_WIDTH_RATIO = 0.66f;

        public static void DrawTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, a);
        }

        public static void DrawSquare(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, d);
            Gizmos.DrawLine(d, a);
        }

        public static void DrawCross(Vector3 center, Vector3 size)
        {
            var dir01 = new Vector3(size.x / 2, size.y / 2, size.z / 2);
            var dir02 = new Vector3(size.x / 2, size.y / 2, size.z / -2);
            var dir03 = new Vector3(size.x / -2, size.y / 2, size.z / 2);
            var dir04 = new Vector3(size.x / -2, size.y / 2, size.z / -2);

            Gizmos.DrawLine(center + dir01, center - dir01);
            Gizmos.DrawLine(center + dir02, center - dir02);
            Gizmos.DrawLine(center + dir03, center - dir03);
            Gizmos.DrawLine(center + dir04, center - dir04);
        }

        public static void DrawPyramid(Vector3 bottom, Vector3 top, float width)
        {
            var axis = top - bottom;

            if (Vector3.zero.IsSimilar(axis) || width.IsSimilar(0))
                return;

            var cross = axis.z.IsSimilar(0) ? Vector3.forward : Vector3.up;

            var base01 = Quaternion.AngleAxis(45, axis) * Vector3.Cross(cross, axis).normalized * width / 2;
            var base02 = Vector3.Cross(base01, axis).normalized * width / 2;

            DrawSquare(bottom + base01, bottom + base02, bottom - base01, bottom - base02);

            Gizmos.DrawLine(bottom + base01, top);
            Gizmos.DrawLine(bottom + base02, top);
            Gizmos.DrawLine(bottom - base01, top);
            Gizmos.DrawLine(bottom - base02, top);
        }

        public static void DrawCubeWithDiagonals(Vector3 center, Vector3 size)
        {
            Gizmos.DrawWireCube(center, size);
            DrawCross(center, size);
        }

        public static void DrawArrow(Vector3 from, Vector3 to, float headSize = ARROW_HEAD_SIZE)
        {
            var length = (to - from).magnitude;

            if (length.IsSimilar(0))
                return;

            var axis = (to - from).normalized;

            headSize = Mathf.Min(length, Mathf.Max(headSize, 0));
            var center = from + axis * (length - headSize);

            DrawPyramid(center, to, headSize * ARROW_HEAD_TO_WIDTH_RATIO);
            Gizmos.DrawLine(from, center);
        }

        public static void DrawPath(Path path)
        {
            if (path == null)
                return;

            DrawPath(path.ToArray());
        }

        public static void DrawPath(Vector3[] path)
        {
            if (path.Length < 2)
                return;

            for (var i = 1; i < path.Length; i++)
                Gizmos.DrawLine(path[i - 1], path[i]);

            DrawArrow(path[^2], path[^1]);
        }
    }
}
