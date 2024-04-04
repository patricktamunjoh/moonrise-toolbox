using System;

namespace MoonriseGames.Toolbox.Utilities
{
    public static class TypeUtility
    {
        public static T[] GetEnumValues<T>()
            where T : Enum => Enum.GetValues(typeof(T)) as T[];
    }
}
