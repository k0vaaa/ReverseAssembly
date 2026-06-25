using Core.Bootstrap;

using Core.Events;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Core
{
    public class SyncEnergyManager : IInitializable
    {
        public SyncEnergyManager()
        {
            Init();
            EventBus.Subscribe<CodeBlockCollectedEvent>(e => AddEnergy(e.Energy));
        }

        public float MaxEnergy { get; private set; } = 100f;
        public float JumpCost { get; private set; } = 15f;
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

        public void SetEnergy(float energy)
        {
            CurrentEnergy = Mathf.Clamp(energy, 0, MaxEnergy);
            RaiseEnergyChangedEvent();
        }

        private float GetPercent() => Mathf.Clamp(CurrentEnergy / MaxEnergy, 0, 1);

        private void RaiseEnergyChangedEvent()
        {
            EventBus.Raise(new SyncEnergyChangedEvent
            {
                CurrentEnergy = CurrentEnergy,
                MaxEnergy = MaxEnergy,
                EnergyPercent = GetPercent()
            });
        }
    }
}
