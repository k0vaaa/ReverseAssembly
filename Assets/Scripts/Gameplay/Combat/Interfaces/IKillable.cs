using UnityEngine.Events;

namespace Gameplay.Combat.Interfaces
{
    public interface IKillable
    {
        public void Die();
        public UnityEvent OnDeath { get;}
    }
}