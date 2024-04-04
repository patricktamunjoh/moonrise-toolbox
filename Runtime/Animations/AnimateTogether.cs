using System;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateTogether : IAnimateDirectional
    {
        private List<IAnimateDirectional> Animations { get; }

        public bool IsPaused => Animations.Any(x => x.IsPaused) && Animations.All(x => x.IsPaused || x.IsStarted.Not());
        public bool IsStarted => Animations.Any(x => x.IsStarted);

        private bool IsCompletable { get; set; }

        public event Action OnComplete;

        public AnimateTogether(params IAnimateDirectional[] animations)
        {
            Animations = new List<IAnimateDirectional>(animations);

            foreach (var animation in Animations)
                animation.OnComplete += CheckForCompletion;
        }

        private void CheckForCompletion()
        {
            if (IsCompletable.Not() || Animations.Any(x => x.IsStarted))
                return;

            IsCompletable = false;
            OnComplete?.Invoke();
        }

        public void SetDirection(bool isReversed)
        {
            foreach (var animation in Animations)
                animation.SetDirection(isReversed);
        }

        public void Start()
        {
            foreach (var animation in Animations)
                animation.Start();

            IsCompletable = true;
            CheckForCompletion();
        }

        public void Restart()
        {
            foreach (var animation in Animations)
                animation.Restart();

            IsCompletable = true;
            CheckForCompletion();
        }

        public void Stop()
        {
            IsCompletable = false;

            foreach (var animation in Animations)
                animation.Stop();
        }

        public void Pause()
        {
            foreach (var animation in Animations)
                animation.Pause();
        }

        public void Resume()
        {
            foreach (var animation in Animations)
                animation.Resume();
        }

        public void Reset()
        {
            IsCompletable = false;

            foreach (var animation in Animations)
                animation.Reset();
        }
    }
}
