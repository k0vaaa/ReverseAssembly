using UnityEngine.Events;

namespace Gameplay.Combat.Interfaces
{
    public interface IHealthChange
    {
        public UnityEvent<float, float> onStabilityChanged {get;}
    }
}