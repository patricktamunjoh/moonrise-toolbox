using System.Linq;
using System.Text.RegularExpressions;
using MoonriseGames.Toolbox.Localization;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class StringExtensions
    {
        public static string Localized(this string key) => LocalizationService.Unit.GetLocalization(key);

        public static string Localized(this string key, string fallback) => LocalizationService.Unit.GetLocalization(key, fallback);

        public static string TitleCase(this string text) =>
            string.Join(" ", text.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(w => char.ToUpper(w[0]) + w[1..]));

        public static string TrimIndents(this string text) => Regex.Replace(text, @"\n\s+", "\n");

        public static string Clamped(this string text, int maxLength, string ellipsis = "")
        {
            if (text.Length <= maxLength)
                return text;

            if (maxLength <= ellipsis.Length)
                return string.Empty;

            return text[..(maxLength - ellipsis.Length)] + ellipsis;
        }
    }
}
