using Core.Events;

namespace Gameplay.Events
{
    public class PlayerStabilityChangedEvent : IEvent
    {
        public bool IsGlitched;
        public float StabilityPercent;
    }
}