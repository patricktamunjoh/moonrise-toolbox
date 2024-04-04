using System;
using System.Linq;
using UnityEngine;

namespace MoonriseGames.Toolbox.Paths
{
    public class BezierPath : Path
    {
        public BezierPath(Vector3[] controls, int pointCount)
            : base(GetPoints(controls, pointCount)) { }

        private static Vector3[] GetPoints(Vector3[] controls, int pointCount)
        {
            if (controls == null || controls.Length < 1)
                throw new ArgumentException("Controls contain less than one point.");

            return Enumerable.Range(0, pointCount).Select(x => GetPoint(controls, x / (float)(pointCount - 1))).ToArray();
        }

        private static Vector3 GetPoint(Vector3[] controls, float progress)
        {
            var controlsCount = controls.Length;
            controls = controls.ToArray();

            while (controlsCount > 1)
            {
                for (var i = 0; i < controlsCount - 1; i++)
                    controls[i] += (controls[i + 1] - controls[i]) * progress;
                controlsCount--;
            }

            return controls[0];
        }
    }
}
