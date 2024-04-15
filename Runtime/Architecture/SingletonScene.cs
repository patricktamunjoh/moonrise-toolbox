using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Architecture
{
    /// <summary>Base class for <see cref="MonoBehaviour"/> to exist as a singleton throughout a single scene.</summary>
    [DisallowMultipleComponent]
    public abstract class SingletonScene<T> : MonoBehaviour
    {
        private static T Instance { get; set; }
        public static T Unit => GetInstance();

        private bool IsInstance { get; set; }

        private static T GetInstance()
        {
            if (Instance.IsNull().Not())
                return Instance;

            Instance = FindObjectsOfType<MonoBehaviour>(true).OfType<T>().FirstOrDefault();

            if (Instance.IsNull())
                throw new SingletonNotAvailableException($"No instance of {typeof(T).Name} found in the active hierarchy.");

            return Instance;
        }

        private void Awake()
        {
            if (Application.isPlaying && Equals(Unit).Not())
                Destroy(gameObject);

            if (Application.isPlaying && Equals(Unit))
                AwakeInstance();
        }

        private void OnDestroy()
        {
            if (Application.isPlaying && IsInstance)
                OnDestroyInstance();
        }

        protected virtual void AwakeInstance()
        {
            IsInstance = true;
        }

        protected virtual void OnDestroyInstance() { }
    }
}
