using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class RectExtensions
    {
        public static IEnumerable<Rect> DivideHorizontal(this Rect rect, float space, int segmentCount) =>
            rect.DivideHorizontal(space, Enumerable.Repeat(null as float?, Mathf.Max(0, segmentCount)).ToArray());

        public static IEnumerable<Rect> DivideHorizontal(this Rect rect, float space, params float?[] segments)
        {
            var excessWidth = Mathf.Max(0, rect.width - segments.Sum(x => x ?? 0) - space * (segments.Length - 1));

            var flexSegmentCount = segments.Count(x => !x.HasValue);
            var flexSegmentWidth = excessWidth / flexSegmentCount;

            return segments.Aggregate(
                new List<Rect>(),
                (list, segment) =>
                {
                    var x = list.Count == 0 ? rect.x : list[^1].x + list[^1].width + space;
                    list.Add(new Rect(x, rect.y, segment ?? flexSegmentWidth, rect.height));
                    return list;
                }
            );
        }

        public static Rect Inset(this Rect rect, float left = 0, float top = 0, float right = 0, float bottom = 0) =>
            new Rect(rect.x + left, rect.y + top, rect.width - left - right, rect.height - top - bottom);
    }
}
