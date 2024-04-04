using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MoonriseGames.Toolbox.Tests.Utilities.Functions
{
    public static class Function
    {
        public static void ClearScene()
        {
            foreach (var transform in Object.FindObjectsOfType<Transform>(true).Where(x => x))
                Object.DestroyImmediate(transform.gameObject);
        }
    }
}
