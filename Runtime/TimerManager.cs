using System.Collections.Generic;

namespace UnityUtils.Timer
{
    public static class TimerManager
    {
        static readonly List<Timer> _Timers = new();

        public static void RegisterTimer(Timer timer) => _Timers.Add(timer);
        public static void DeregisterTimer(Timer timer) => _Timers.Remove(timer);

        public static void UpdateTimers()
        {
            if (_Timers.Count == 0) return;

            for (int i = _Timers.Count - 1; i >= 0; i--)
            {
                _Timers[i].Tick();
            }
        }

        public static void Clear()
        {
            for (int i = _Timers.Count - 1; i >= 0; i--)
            {
                _Timers[i].Dispose();
            }
            _Timers.Clear();
        }
    }
}