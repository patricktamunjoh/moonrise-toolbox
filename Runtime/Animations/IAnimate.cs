using System;

namespace MoonriseGames.Toolbox.Animations
{
    public interface IAnimate
    {
        event Action OnComplete;

        bool IsPaused { get; }
        bool IsStarted { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void Restart();

        void Reset();
    }
}
