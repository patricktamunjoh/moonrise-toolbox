using System.IO;
using MoonriseGames.Toolbox.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoonriseGames.Toolbox.Editor.Utilities
{
    public static class PathUtility
    {
        public static string GetUniqueColocatedAssetPath(string name, Object sibling)
        {
            var fullPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(sibling));
            var path = Path.Combine("Assets", Path.GetRelativePath(Application.dataPath, fullPath));

            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, $"{name.TitleCase()}.asset"));
        }
    }
}
