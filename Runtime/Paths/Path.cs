using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Paths
{
    public class Path : IEnumerable<Vector3>
    {
        private (Vector3 point, float progress)[] Positions { get; }

        public float Length { get; }
        public int PointCount => Positions.Length;

        public Path(Vector3[] points)
        {
            if (points == null || points.Length < 2)
                throw new ArgumentException("Points contain less than two points.");

            Length = GetTotalLength(points);
            Positions = GetPositions(points, Length);
        }

        public Vector3 this[int index] => Positions[index].point;

        public Vector3 this[float progress] => Sample(progress);

        private float GetTotalLength(Vector3[] points) => points.Zip(points.Skip(1), Vector3.Distance).Sum();

        private (Vector3 point, float progress)[] GetPositions(Vector3[] points, float totalLength)
        {
            var positions = new (Vector3 point, float progress)[points.Length];

            positions[0] = (points[0], 0);
            positions[^1] = (points[^1], 1);

            for (var i = 1; i < points.Length; i++)
            {
                var progress = positions[i - 1].progress + Vector3.Distance(points[i - 1], points[i]) / totalLength;
                positions[i] = (points[i], progress);
            }

            return positions;
        }

        public Vector3 Sample(float progress)
        {
            if (progress >= 1)
                return Positions[^1].point;

            var rightIndex = Positions.Indexed().FirstOrDefault(x => x.value.progress >= progress).index;

            if (rightIndex == 0)
                return Positions[0].point;

            var intervalProgress = GetProgressInInterval(progress, Positions[rightIndex - 1].progress, Positions[rightIndex].progress);
            return Vector2.Lerp(Positions[rightIndex - 1].point, Positions[rightIndex].point, intervalProgress);
        }

        public Path Optimize(float tolerance)
        {
            var subset = DouglasPeucker(Positions.Select(x => x.point).ToList(), tolerance);
            return new Path(subset.ToArray());
        }

        private IEnumerable<Vector3> DouglasPeucker(List<Vector3> points, float tolerance)
        {
            if (points.Count <= 2)
                return points;

            var (distance, index) = points
                .Select(x => x.DistanceToLine(points[0], points[^1]))
                .Indexed()
                .Skip(1)
                .SkipLast(1)
                .OrderBy(x => x.value)
                .Last();

            if (distance <= tolerance)
                return points.Take(1).Concat(points.TakeLast(1));

            return DouglasPeucker(points.Take(index + 1).ToList(), tolerance)
                .SkipLast(1)
                .Concat(DouglasPeucker(points.TakeLast(points.Count - index).ToList(), tolerance));
        }

        private float GetProgressInInterval(float x, float from, float to) => to <= from ? 0 : Mathf.Clamp01((x - from) / (to - from));
        
        public IEnumerator<Vector3> GetEnumerator() => Positions.Select(x => x.point).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
