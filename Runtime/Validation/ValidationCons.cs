using UnityEngine.Rendering;

namespace MoonriseGames.Toolbox.Validation
{
    internal static class ValidationCons
    {
        public static string[] IgnoredNamespaces { get; } = { "Unity", "TMPro" };

        public static string[] IncludedClasses { get; } = { nameof(SortingGroup) };
    }
}
