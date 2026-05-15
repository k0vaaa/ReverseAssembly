using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Gameplay.Events;

namespace Gameplay.Core
{
    public class SyncEnergyManager : IInjectable, IInitializable
    {
        public float MaxEnergy { get; private set; } = 100f;
        public float JumpCost { get; private set; } = 30f;
        public float CurrentEnergy { get; private set; }

        public void Init()
        {
            CurrentEnergy = MaxEnergy;
            RaiseEnergyChangedEvent();
        }

        public bool TryConsumeEnergy()
        {
            if (CurrentEnergy >= JumpCost)
            {
                CurrentEnergy -= JumpCost;
                RaiseEnergyChangedEvent();
                return true;
            }
            return false;
        }

        public void AddEnergy(float amount)
        {
            CurrentEnergy += amount;
            if (CurrentEnergy > MaxEnergy)
            {
                CurrentEnergy = MaxEnergy;
            }
            RaiseEnergyChangedEvent();
        }

        private void RaiseEnergyChangedEvent()
        {
            EventBus.Raise(new SyncEnergyChangedEvent
            {
                CurrentEnergy = CurrentEnergy,
                MaxEnergy = MaxEnergy
            });
        }
    }
}
