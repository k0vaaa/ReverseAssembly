using Gameplay.Combat.Interfaces;

namespace Gameplay.Combat.Offensive.Base
{
    public interface IDamaging
    {
        void DoDamage(IDamageable damageable);
    }
}