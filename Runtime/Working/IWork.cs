using System;

namespace MoonriseGames.Toolbox.Working
{
    public interface IWork
    {
        event Action<float> OnProgress;

        event Action OnStart;
        event Action OnStop;

        event Action OnPause;
        event Action OnResume;

        event Action OnComplete;

        bool IsPaused { get; }
        bool IsStarted { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void Restart();
    }
}
