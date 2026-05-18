using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.Helpers;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Combat.Offensive.Skills.Definitions;
using Gameplay.Enemies.States;
using Gameplay.StateMachines.PlayerStates.FightStates;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class PunchSkill : Skill
    {
        
        private readonly PunchSkillDefinition _def;
        private readonly SkillContext _ctx;

        private CollisionDamageDealer _collisionDamageDealer;
        private GameObject _caster;

        public PunchSkill(PunchSkillDefinition def, SkillContext ctx) : base(def)
        {
            _def = def;
            _ctx = ctx;
        }

        public override void Init()
        {
            _caster = _ctx.Caster;
            _collisionDamageDealer = _caster.GetComponentInChildren<HitBox>(true).gameObject
                .AddComponent<CollisionDamageDealer>();
            _collisionDamageDealer.Init(_caster.GetComponent<IDamageable>(), _def.damage);
        }

        protected override void OnTick()
        {
        }

        protected override bool CastAction()
        {
            return true;
        }

        public void ClearCollider()
        {
            _collisionDamageDealer.ClearEnemiesList();
        }
    }
}