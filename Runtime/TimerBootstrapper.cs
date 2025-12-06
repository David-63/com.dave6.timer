using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityUtils.LowLevel;

namespace UnityUtils.Timer
{
    internal static class TimerBootstrapper
    {
        static PlayerLoopSystem timerSystem;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize()
        {
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.Log("Improved Timers not initialized, unable to register TimerManager into the Update loop.");
                return;
            }

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
            PlayerLoopUtils.PrintPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;
#endif
            static void OnPlayModeState(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    RemoveTimerManager<Update>(ref currentPlayerLoop);
                    PlayerLoop.SetPlayerLoop(currentPlayerLoop);

                    TimerManager.Clear();
                }
            }
        }

        static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int idx)
        {
            timerSystem = new PlayerLoopSystem()
            {
                type = typeof(TimerManager),
                subSystemList = null,
                updateDelegate = TimerManager.UpdateTimers,
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loop, in timerSystem, idx);
        }

        static void RemoveTimerManager<T>(ref PlayerLoopSystem loop)
        {
            PlayerLoopUtils.RemoveSystem<T>(ref loop, in timerSystem);
        }
    }
}