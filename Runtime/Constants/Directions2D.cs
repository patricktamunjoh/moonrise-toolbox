using System.Collections.Generic;
using UnityEngine;

namespace MoonriseGames.Toolbox.Constants
{
    public static class Directions2D
    {
        public static IEnumerable<Vector2Int> AXIS
        {
            get
            {
                yield return Vector2Int.up;
                yield return Vector2Int.down;
                yield return Vector2Int.left;
                yield return Vector2Int.right;
            }
        }

        public static IEnumerable<Vector2Int> EVERY_45
        {
            get
            {
                yield return Vector2Int.up;
                yield return Vector2Int.down;
                yield return Vector2Int.left;
                yield return Vector2Int.right;

                yield return new Vector2Int(1, 1);
                yield return new Vector2Int(-1, -1);
                yield return new Vector2Int(-1, 1);
                yield return new Vector2Int(1, -1);
            }
        }
    }
}
