using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Utilities.Factories
{
    public static class LocalizationFactory
    {
        public static Translation GetTranslation(string header, params string[] lines)
        {
            var text = header + '\n' + string.Join('\n', lines);
            var file = new TextAsset(text);
            var translation = ScriptableObject.CreateInstance<Translation>();

            translation.SetNonPublicField(new[] { file });
            translation.RefreshLanguageData();

            return translation;
        }
    }
}
