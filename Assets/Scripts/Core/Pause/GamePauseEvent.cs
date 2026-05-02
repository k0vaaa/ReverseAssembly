using Core.Events;

namespace Core.Pause
{
    public class GamePauseEvent : IEvent
    {
        public bool IsPaused;

        public GamePauseEvent() {}

        public GamePauseEvent(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}