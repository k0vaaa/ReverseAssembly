using UnityEngine;

namespace Gameplay.Enemies.Behaviors
{
    public abstract class EnemyBehaviorStrategy : ScriptableObject
    {
        public abstract void InitializeBehavior(EnemyBrain brain, AIController controller);
    }
}
