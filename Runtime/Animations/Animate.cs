using System;
using MoonriseGames.Toolbox.Working;

namespace MoonriseGames.Toolbox.Animations
{
    public abstract class Animate : IAnimateDirectional
    {
        protected IWork Work { get; set; }
        protected AnimationConfig Config { get; }

        public bool IsPaused => Work.IsPaused;
        public bool IsStarted => Work.IsStarted;

        private bool IsStartedOnce { get; set; }
        protected bool IsReversed { get; private set; }

        public abstract event Action OnComplete;

        protected Animate(AnimationConfig config)
        {
            Config = config;
        }

        protected abstract void RestoreInitialState();

        public void SetDirection(bool isReversed) => IsReversed = isReversed;

        public void Start()
        {
            if (IsStartedOnce && Config.Playback == AnimationPlayback.ForwardOnce)
                return;

            IsStartedOnce = true;
            Work?.Start();
        }

        public void Restart()
        {
            if (IsStartedOnce && Config.Playback == AnimationPlayback.ForwardOnce)
                return;

            IsStartedOnce = true;
            Work?.Restart();
        }

        public void Reset()
        {
            IsStartedOnce = false;

            Work?.Stop();
            RestoreInitialState();
        }

        public void Stop() => Work?.Stop();

        public void Pause() => Work?.Pause();

        public void Resume() => Work?.Resume();
    }
}
