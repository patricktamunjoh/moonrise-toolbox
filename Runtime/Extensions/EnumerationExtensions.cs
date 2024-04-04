using System.Collections.Generic;
using System.Linq;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class EnumerationExtensions
    {
        public static IEnumerable<(T value, int index)> Indexed<T>(this IEnumerable<T> iterator) => iterator.Select((x, i) => (x, i));
    }
}
