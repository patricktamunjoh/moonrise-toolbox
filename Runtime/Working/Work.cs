using System;
using System.Collections;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Working
{
    public abstract class Work : IWork
    {
        private MonoBehaviour Context { get; }
        private IEnumerator Execution { get; set; }

        protected bool UseUnscaledTime { get; }

        public bool IsPaused { get; private set; }
        public bool IsStarted => Execution != null;

        public event Action<float> OnProgress;

        public event Action OnStart;
        public event Action OnStop;

        public event Action OnPause;
        public event Action OnResume;

        public event Action OnComplete;

        protected Work(MonoBehaviour context, bool useUnscaledTime)
        {
            Context = context;
            UseUnscaledTime = useUnscaledTime;
        }

        protected abstract IEnumerator GetExecutionCoroutine();

        protected void OnExecutionCompleted()
        {
            Execution = null;
            OnComplete?.Invoke();
        }

        protected void OnExecutionProgress(float p) => OnProgress?.Invoke(p);

        public void Start()
        {
            if (IsStarted)
            {
                Resume();
                return;
            }

            if (Context == null || Context.isActiveAndEnabled.Not())
                return;

            IsPaused = false;
            Context.StartCoroutine(Execution = GetExecutionCoroutine());
            OnStart?.Invoke();
        }

        public void Stop()
        {
            if (IsStarted.Not())
                return;

            if (Context != null)
                Context.StopCoroutine(Execution);

            IsPaused = false;
            Execution = null;
            OnStop?.Invoke();
        }

        public void Pause()
        {
            if (IsPaused || IsStarted.Not())
                return;

            IsPaused = true;
            OnPause?.Invoke();
        }

        public void Resume()
        {
            if (IsPaused.Not() || IsStarted.Not())
                return;

            IsPaused = false;
            OnResume?.Invoke();
        }

        public void Restart()
        {
            if (IsStarted)
                Execution = null;

            if (IsStarted && Context != null)
                Context.StopCoroutine(Execution);

            Start();
        }
    }
}
