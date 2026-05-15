using Core.Events;

namespace Gameplay.Events
{
    public struct SyncEnergyChangedEvent : IEvent
    {
        public float CurrentEnergy;
        public float MaxEnergy;
    }
}
