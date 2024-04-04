using System;

namespace MoonriseGames.Toolbox.Tests.Utilities.Extensions
{
    internal static class EnumExtensions
    {
        public static T[] EnumValues<T>(this Type enumType) => Enum.GetValues(enumType) as T[];
    }
}
