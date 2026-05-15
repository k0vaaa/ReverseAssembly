using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.ScriptableObjects;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class PunchSkill : Skill
    {
        private readonly Sword _sword;

        public PunchSkill(SkillData skillData, Transform a, Transform b, GameObject sword, IDamageable self) :
            base(skillData)
        {
            _sword = sword.AddComponent<Sword>();
            _sword.Init(self,skillData.damage);
        }

        public override void Cast()
        {
            base.Cast();
            _sword.ClearEnemiesList();
        }
    }
}