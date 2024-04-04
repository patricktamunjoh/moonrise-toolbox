using System.Linq;
using System.Text.RegularExpressions;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Editor.Utilities;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.MenuItems
{
    public static class LocalizationMenuItems
    {
        [MenuItem("Assets/Toolbox/Localization/Sprites to Translation", priority = Orders.ASSETS)]
        public static void GenerateAudioEffect()
        {
            var sprites = Selection
                .objects.OfType<Texture2D>()
                .Select(AssetDatabase.GetAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Sprite>)
                .Where(x => x)
                .ToArray();

            if (sprites.Length == 0)
                return;

            var name = Regex.Match(sprites[0].name, @"[^\d]*").Value;
            var so = ScriptableObject.CreateInstance<TranslationSprites>();

            so.SetNonPublicField(sprites.Select(x => new TranslationValue<Sprite>(Application.systemLanguage, x)).ToArray());
            so.SetNonPublicField(sprites[0]);

            AssetDatabase.CreateAsset(so, PathUtility.GetUniqueColocatedAssetPath(name, sprites[0]));
        }

        [MenuItem("Assets/Toolbox/Localization/Sprites to Translation", true)]
        public static bool ValidateGenerateAudioEffect() => Selection.objects.OfType<Texture2D>().Any();
    }
}
