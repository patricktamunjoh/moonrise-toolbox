using System.Collections.Generic;
using UnityEngine;

namespace MoonriseGames.Toolbox.Constants
{
    public static class Directions3D
    {
        public static IEnumerable<Vector3Int> AXIS
        {
            get
            {
                yield return Vector3Int.up;
                yield return Vector3Int.down;
                yield return Vector3Int.left;
                yield return Vector3Int.right;
                yield return Vector3Int.forward;
                yield return Vector3Int.back;
            }
        }

        public static IEnumerable<Vector3Int> EVERY_45
        {
            get
            {
                yield return Vector3Int.up;
                yield return Vector3Int.down;
                yield return Vector3Int.left;
                yield return Vector3Int.right;
                yield return Vector3Int.forward;
                yield return Vector3Int.back;

                yield return new Vector3Int(1, 1, 1);
                yield return new Vector3Int(1, 1, -1);
                yield return new Vector3Int(1, -1, 1);
                yield return new Vector3Int(1, -1, -1);
                yield return new Vector3Int(-1, 1, 1);
                yield return new Vector3Int(-1, 1, -1);
                yield return new Vector3Int(-1, -1, 1);
                yield return new Vector3Int(-1, -1, -1);
            }
        }
    }
}
