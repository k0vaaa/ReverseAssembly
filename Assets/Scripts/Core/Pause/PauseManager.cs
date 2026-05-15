using Core.Events;
using UnityEngine;

namespace Core.Pause
{
    public static class PauseManager
    {
        public static bool IsPaused;

        public static void SetPause(bool isPaused)
        {
            if (IsPaused == isPaused) return;
            IsPaused = isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            
            EventBus.Raise(new GamePauseEvent
            {
                IsPaused = isPaused
            });
        }
        
        public static void TogglePause()
        {
            SetPause(!IsPaused);
        }
    }
}