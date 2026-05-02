using UnityEngine.Events;

namespace Gameplay.Combat.Interfaces
{
    public interface IStabilityChange
    {
        public UnityEvent<float, float> OnStabilityChanged {get;}
    }
}