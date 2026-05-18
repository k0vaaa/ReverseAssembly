using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.ScriptableObjects;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class HeavyAttack : Skill
    {
        private readonly CollisionDamageDealer _collisionDamageDealer;

        public HeavyAttack(AbilityDefinition abilityDefinition, GameObject sword, IDamageable self) : base(abilityDefinition)
        {
            _collisionDamageDealer = sword.AddComponent<CollisionDamageDealer>();
            _collisionDamageDealer.Init(self,abilityDefinition.damage);
        }

        protected override void OnTick()
        {
            
        }

        protected override bool CastAction()
        {
            _collisionDamageDealer.ClearEnemiesList();
            return true;
        }
    }
}