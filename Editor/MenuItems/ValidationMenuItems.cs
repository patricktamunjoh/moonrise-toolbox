using System.Linq;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Editor.Utilities;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Validation;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.MenuItems
{
    public static class ValidationMenuItems
    {
        [MenuItem("Assets/Toolbox/Validation/Validate Assets", priority = Orders.ASSETS)]
        public static void ValidateAssets()
        {
            EditorAndAssetUtility.ClearConsole();
            LoadValidationScene();

            var isSuccess = true;

            foreach (var (component, path) in EditorAndAssetUtility.GetAllPrefabComponentsInProject<MonoBehaviour>("Scanning Prefabs"))
                ValidateObject(component, AssetDatabase.LoadAssetAtPath<GameObject>(path), ref isSuccess);

            foreach (var so in EditorAndAssetUtility.GetAllAssetsInProject<ScriptableObject>("Scanning Scriptable Objects"))
                ValidateObject(so, so, ref isSuccess);

            if (isSuccess)
                Debug.Log("<b>Validation successful</b>");
            else
                Debug.LogError("<b>Validation failed</b>");
        }

        private static void LoadValidationScene()
        {
            EditorAndAssetUtility.OpenScene("Validation", out var isSuccess);

            if (isSuccess.Not())
            {
                const string missingSceneMessage =
                    @"No validation scene found. Continuing validation in the current scene.
                    To configure where the validation runs, create a scene ""Validation"" within the project.";

                Debug.Log(missingSceneMessage.TrimIndents());
            }
        }

        private static void ValidateObject(Object target, Object asset, ref bool isSuccess)
        {
            var validator = new Validator();

            validator.Validate(target);

            if (validator.Result.IsSuccess.Not())
            {
                isSuccess = false;

                var assetPath = target == asset || asset == null ? target.name : $"{asset.name}/{target.name}";
                var issues = string.Join('\n', validator.Result.Issues.Select(x => $"{x.Message} @ {x.Path}"));
                var errorMessage = $"Validation failed for {assetPath}\n{issues}";

                Debug.LogError(errorMessage.TrimIndents(), asset);
            }
        }
    }
}
