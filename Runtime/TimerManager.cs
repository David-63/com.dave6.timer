using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace UnityUtils.Timer
{
    public static class TimerManager
    {
        static readonly List<Timer> timers = new();

        public static void RegisterTimer(Timer timer) => timers.Add(timer);
        public static void DeregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimers()
        {
            if (timers.Count == 0) return;

            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i].Tick();
            }
        }

        public static void Clear()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i].Dispose();
            }
            timers.Clear();
        }
    }
}