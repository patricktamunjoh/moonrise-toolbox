using System;
using MoonriseGames.Toolbox.Working;
using UnityEngine;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateBetween : Animate
    {
        private Action<float> ApplyProgress { get; }

        public override event Action OnComplete;

        public AnimateBetween(MonoBehaviour context, float duration, AnimationConfig config, Action<float> onProgress)
            : base(config)
        {
            ApplyProgress = onProgress;

            Work = new WorkOnce(context, GetWorkDuration(config, duration), config.UseUnscaledTime);
            Work.OnProgress += OnWorkProgress;
            Work.OnComplete += OnWorkComplete;
        }

        protected override void RestoreInitialState() => ApplyProgress?.Invoke(IsReversed ? 1 : 0);

        private float GetWorkDuration(AnimationConfig config, float initialDuration)
        {
            if (config.Playback == AnimationPlayback.PingPong)
                return initialDuration * 2;

            return initialDuration;
        }

        private void OnWorkProgress(float progress)
        {
            if (IsReversed)
                progress = 1 - progress;

            if (Config.Playback == AnimationPlayback.PingPong)
                progress = Mathf.PingPong(progress * 2, 1);

            if (Config.Interpolation == AnimationInterpolation.EaseInOut)
                progress = Mathf.SmoothStep(0, 1, progress);

            ApplyProgress?.Invoke(progress);
        }

        private void OnWorkComplete()
        {
            switch (Config.Playback)
            {
                case AnimationPlayback.Forward:
                case AnimationPlayback.ForwardOnce:
                    OnComplete?.Invoke();
                    break;

                case AnimationPlayback.Loop:
                case AnimationPlayback.PingPong:
                    Restart();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
