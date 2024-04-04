using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithR(this Color color, float r) => new(r, color.g, color.b, color.a);

        public static Color WithG(this Color color, float g) => new(color.r, g, color.b, color.a);

        public static Color WithB(this Color color, float b) => new(color.r, color.g, b, color.a);

        public static Color WithAlpha(this Color color, float a) => new(color.r, color.g, color.b, a);

        public static bool IsSimilar(this Color color, Color other) =>
            color.r.IsSimilar(other.r) && color.g.IsSimilar(other.g) && color.b.IsSimilar(other.b) && color.a.IsSimilar(other.a);
    }
}
