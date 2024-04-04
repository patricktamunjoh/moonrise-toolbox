using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer) => mask == (mask | (1 << layer));
    }
}
