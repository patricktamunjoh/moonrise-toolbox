using System.Reflection;
using MoonriseGames.Toolbox.Architecture;

namespace MoonriseGames.Toolbox.Tests.Utilities.Extensions
{
    public static class SingletonExtensions
    {
        public static void SetAsSingletonInstance<T>(this T instance)
        {
            var propertyInfo = typeof(SingletonScene<T>).GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            propertyInfo.SetValue(null, instance);
        }
    }
}
