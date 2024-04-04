using System.Linq;
using System.Text.RegularExpressions;
using MoonriseGames.Toolbox.Audio;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Editor.Utilities;
using MoonriseGames.Toolbox.Extensions;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.MenuItems
{
    public static class AudioMenuItems
    {
        [MenuItem("Assets/Toolbox/Audio/Audio Clips to Effect", priority = Orders.ASSETS)]
        public static void GenerateAudioEffect()
        {
            var sounds = Selection.objects.OfType<AudioClip>().ToList();
            var name = Regex.Match(sounds[0].name, @"[^\d]*").Value;
            var so = ScriptableObject.CreateInstance<AudioEffect>();

            so.SetNonPublicField(sounds.ToArray());
            AssetDatabase.CreateAsset(so, PathUtility.GetUniqueColocatedAssetPath(name, sounds[0]));
        }

        [MenuItem("Assets/Toolbox/Audio/Audio Clips to Effect", true)]
        public static bool ValidateGenerateAudioEffect() => Selection.objects.OfType<AudioClip>().Any();

        [MenuItem("Assets/Toolbox/Audio/Audio Clips to Ramp", priority = Orders.ASSETS + 1)]
        public static void GenerateAudioRamp()
        {
            var sounds = Selection.objects.OfType<AudioClip>().ToList();
            var name = Regex.Match(sounds[0].name, @"[^\d]*").Value + " Ramp";
            var so = ScriptableObject.CreateInstance<AudioRamp>();

            so.SetNonPublicField(sounds.ToArray());
            AssetDatabase.CreateAsset(so, PathUtility.GetUniqueColocatedAssetPath(name, sounds[0]));
        }

        [MenuItem("Assets/Toolbox/Audio/Audio Clips to Ramp", true)]
        public static bool ValidateGenerateAudioRamp() => Selection.objects.OfType<AudioClip>().Any();
    }
}
