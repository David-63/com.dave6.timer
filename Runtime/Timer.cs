using System;
using UnityEngine;

namespace UnityUtils.Timer
{
    public abstract class Timer : IDisposable
    {
        public float CurrentTime { get; protected set; }
        public bool IsRunning { get; private set; }

        protected float InitialTime;

        public float Progress => Mathf.Clamp(CurrentTime / InitialTime, 0, 1);

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };


        protected Timer(float value) => InitialTime = value;
        public void Start()
        {
            CurrentTime = InitialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnTimerStart?.Invoke();
            }
        }
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimerStop?.Invoke();
            }
        }
        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public virtual void Reset() => CurrentTime = InitialTime;
        public virtual void Reset(float newTime)
        {
            InitialTime = newTime;
            Reset();
        }

        #region Dispose
        bool disposed;
        ~Timer() => Dispose(false);

        // Call Dispose to ensure deregistration of the timer from the TimerManager
        // when the consumer is done with the timer or being destroyed
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                TimerManager.DeregisterTimer(this);
            }
            disposed = true;
        }
        #endregion
    }

    public static class TimerExtensions
    {
        /// <summary>
        /// 공통 함수로 제공되지만, Tick이 구현된 타이머에서만 작동함!
        /// 타이머를 다시 돌리는 함수
        /// </summary>
        /// <param name="target"></param>
        public static void RestartTimer(this Timer target)
        {
            if (target.IsRunning)
            {
                target.Reset();
                target.Resume();
            }
            else
            {
                target.Start();
            }
        }
    }
}