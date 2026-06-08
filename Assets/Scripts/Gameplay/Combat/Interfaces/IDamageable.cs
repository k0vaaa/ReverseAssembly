using Gameplay.Combat.Offensive.Base;
using UnityEngine.Events;

namespace Gameplay.Combat.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(Damage damage);
        public UnityEvent onHit { get; }
    }
}