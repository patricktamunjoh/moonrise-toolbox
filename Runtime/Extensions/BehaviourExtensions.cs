using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class BehaviourExtensions
    {
        public static void DestroyGameObject(this Behaviour behaviour)
        {
            if (!behaviour)
                return;

            if (Application.isPlaying)
                Object.Destroy(behaviour.gameObject);
            else
                Object.DestroyImmediate(behaviour.gameObject);
        }
    }
}
