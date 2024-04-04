using System.Collections.Generic;
using System.Reflection;
using MoonriseGames.Toolbox.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.Utilities
{
    public static class EditorAndAssetUtility
    {
        public static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");

            method?.Invoke(new object(), null);
        }

        public static void OpenScene(string name, out bool isSuccess)
        {
            var scenes = AssetDatabase.FindAssets($"t:scene {name}");
            isSuccess = false;

            if (scenes.Length == 0)
                return;

            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(scenes[0]));
            isSuccess = true;
        }

        public static IEnumerable<(GameObject prefab, string path)> GetAllPrefabsInProject(string progressTitle = null)
        {
            var guids = AssetDatabase.FindAssets("t:prefab");

            foreach (var (guid, index) in guids.Indexed())
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var root = PrefabUtility.LoadPrefabContents(path);

                if (progressTitle != null)
                    EditorUtility.DisplayProgressBar(progressTitle, root.name, (float)index / guids.Length);

                yield return (root, path);
                PrefabUtility.UnloadPrefabContents(root);
            }

            if (progressTitle != null)
                EditorUtility.ClearProgressBar();
        }

        public static IEnumerable<T> GetAllAssetsInProject<T>(string progressTitle = null)
            where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

            foreach (var (guid, index) in guids.Indexed())
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(T)) as T;

                if (asset == null)
                    continue;

                if (progressTitle != null)
                    EditorUtility.DisplayProgressBar(progressTitle, asset.name, (float)index / guids.Length);

                yield return asset;
            }

            if (progressTitle != null)
                EditorUtility.ClearProgressBar();
        }

        public static IEnumerable<(T component, string path)> GetAllPrefabComponentsInProject<T>(string progressTitle = null)
            where T : Component
        {
            foreach (var (prefab, path) in GetAllPrefabsInProject(progressTitle))
            foreach (var component in prefab.GetComponentsInChildren(typeof(T), true))
                yield return ((T)component, path);
        }
    }
}
