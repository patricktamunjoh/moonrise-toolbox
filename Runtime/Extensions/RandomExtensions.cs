using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class RandomExtensions
    {
        public static bool Check(this float probability, System.Random random = null) =>
            probability != 0 && (random?.NextDouble() ?? Random.value) <= probability;

        public static T Sample<T>(this IList<T> list, System.Random random = null)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[random?.Next(0, list.Count) ?? Random.Range(0, list.Count)];
        }

        public static T Sample<T>(this IList<T> list, float[] weights, System.Random random = null)
        {
            if (list == null || list.Count == 0)
                return default;

            if (weights == null || list.Count != weights.Length)
                throw new ArgumentException("Weights count does not match list.");

            var cutoff = (random?.NextDouble() ?? Random.value) * weights.Sum();

            for (var i = 0; i < weights.Length; i++)
            {
                cutoff -= weights[i];
                if (cutoff <= 0)
                    return list[i];
            }

            throw new ArgumentException("Weights are invalid.");
        }

        public static T Shuffled<T>(this T list, System.Random random = null)
            where T : IList
        {
            for (var i = 0; i < list.Count; i++)
            {
                var k = random?.Next(i, list.Count) ?? Random.Range(i, list.Count);
                (list[i], list[k]) = (list[k], list[i]);
            }

            return list;
        }
    }
}
