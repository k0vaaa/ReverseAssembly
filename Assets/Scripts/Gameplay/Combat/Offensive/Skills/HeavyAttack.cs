using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.ScriptableObjects;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class HeavyAttack : Skill
    {
        private readonly DamageCollider _damageCollider;

        public HeavyAttack(SkillData skillData, Transform a, Transform b, GameObject sword, IDamageable self) : base(skillData)
        {
            _damageCollider = sword.AddComponent<DamageCollider>();
            _damageCollider.Init(self,skillData.damage);
        }

        protected override void CastAction()
        {
            _damageCollider.ClearEnemiesList();
        }
    }
}