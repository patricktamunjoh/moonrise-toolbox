using System;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateSequence : IAnimate
    {
        private List<IAnimateDirectional> Animations { get; }
        private AnimationPlayback Playback { get; }

        public bool IsPaused => Animations.Count > 0 && CurrentAnimationIndex.HasValue && Animations[CurrentAnimationIndex.Value].IsPaused;
        public bool IsStarted =>
            Animations.Count > 0 && CurrentAnimationIndex.HasValue && Animations[CurrentAnimationIndex.Value].IsStarted;

        private int? CurrentAnimationIndex { get; set; }

        private bool IsStartedOnce { get; set; }
        private bool IsReversed { get; set; }
        private bool IsCompletable { get; set; }

        public event Action OnComplete;

        public AnimateSequence(params IAnimateDirectional[] effects)
            : this(AnimationPlayback.Forward, effects) { }

        public AnimateSequence(AnimationPlayback playback, params IAnimateDirectional[] effects)
        {
            Animations = new List<IAnimateDirectional>(effects);
            Playback = playback;

            foreach (var (effect, index) in Animations.Indexed())
                effect.OnComplete += () => OnEffectComplete(index);
        }

        private void ReturnToSequenceStart()
        {
            IsReversed = false;
            CurrentAnimationIndex = null;
        }

        private void CheckForCompletion()
        {
            if (Playback is not (AnimationPlayback.Forward or AnimationPlayback.ForwardOnce))
                return;

            if (IsCompletable.Not() || CurrentAnimationIndex < Animations.Count)
                return;

            ReturnToSequenceStart();

            IsCompletable = false;
            OnComplete?.Invoke();
        }

        private void OnEffectComplete(int index)
        {
            if (index != CurrentAnimationIndex)
                return;

            if (Playback is AnimationPlayback.Forward or AnimationPlayback.ForwardOnce)
                AdvanceSequenceForward();

            if (Playback is AnimationPlayback.Loop)
                AdvanceSequenceLoop();

            if (Playback is AnimationPlayback.PingPong)
                AdvanceSequencePingPong();

            CheckForCompletion();

            if (IsCompletable.Not())
                return;

            InvokeAtCurrentIndex(a => a.Start());
        }

        private void AdvanceSequencePingPong()
        {
            CurrentAnimationIndex += IsReversed ? -1 : 1;

            if (CurrentAnimationIndex >= Animations.Count)
            {
                CurrentAnimationIndex = Animations.Count - 1;
                IsReversed = true;
            }

            if (CurrentAnimationIndex < 0)
            {
                CurrentAnimationIndex = 0;
                IsReversed = false;
            }
        }

        private void AdvanceSequenceForward() => CurrentAnimationIndex++;

        private void AdvanceSequenceLoop() => CurrentAnimationIndex = (CurrentAnimationIndex + 1) % Animations.Count;

        public void Start()
        {
            if (IsStartedOnce && Playback == AnimationPlayback.ForwardOnce)
                return;

            CurrentAnimationIndex ??= 0;

            IsStartedOnce = true;
            IsCompletable = true;

            InvokeAtCurrentIndex(a => a.Start());
            CheckForCompletion();
        }

        public void Restart()
        {
            if (IsStartedOnce && Playback == AnimationPlayback.ForwardOnce)
                return;

            InvokeAtCurrentIndex(a => a.Stop());
            ReturnToSequenceStart();

            CurrentAnimationIndex = 0;

            IsStartedOnce = true;
            IsCompletable = true;

            InvokeAtCurrentIndex(a => a.Start());
            CheckForCompletion();
        }

        public void Stop()
        {
            IsCompletable = false;

            InvokeAtCurrentIndex(a => a.Stop());
            ReturnToSequenceStart();
        }

        public void Reset()
        {
            IsCompletable = false;
            IsStartedOnce = false;

            foreach (var animation in (Animations as IEnumerable<IAnimate>).Reverse())
                animation.Reset();

            ReturnToSequenceStart();
        }

        public void Pause() => InvokeAtCurrentIndex(a => a.Pause());

        public void Resume() => InvokeAtCurrentIndex(a => a.Resume());

        private void InvokeAtCurrentIndex(Action<IAnimate> action)
        {
            if (CurrentAnimationIndex == null || CurrentAnimationIndex < 0 || CurrentAnimationIndex >= Animations.Count)
                return;

            Animations[CurrentAnimationIndex.Value].SetDirection(IsReversed);
            action(Animations[CurrentAnimationIndex.Value]);
        }
    }
}
