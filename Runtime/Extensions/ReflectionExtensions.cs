using System.Linq;
using System.Reflection;

namespace MoonriseGames.Toolbox.Extensions
{
    internal static class ReflectionExtensions
    {
        public static void SetNonPublicField<T>(this object target, T value) =>
            target
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.FieldType == typeof(T))
                ?.SetValue(target, value);

        public static void SetNonPublicField(this object target, string name, object value) =>
            target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(target, value);
    }
}
