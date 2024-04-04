using UnityEngine;

namespace MoonriseGames.Toolbox.Architecture
{
    /// <summary>Base class for <see cref="MonoBehaviour"/> to exist as a singleton throughout the entire lifetime of the app.</summary>
    public abstract class SingletonGlobal<T> : SingletonScene<T>
    {
        protected override void AwakeInstance()
        {
            if (Application.isPlaying)
                DontDestroyOnLoad(this);
        }
    }
}
