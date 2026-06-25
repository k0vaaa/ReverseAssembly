using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.Helpers;
using Gameplay.Combat.Offensive.Skills.Definitions;
using Gameplay.Controllers.Player;
using Gameplay.StateMachines.PlayerStates.FightStates;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class MeleeSkill : Skill
    {
        [Inject] private FightController _fight;
        private readonly MeleeSkillDefinition _def;
        private readonly SkillContext _ctx;

        private CollisionDamageDealer _collisionDamageDealer;
        private GameObject _caster;

        public MeleeSkill(MeleeSkillDefinition def, SkillContext ctx) : base(def)
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
            if(_fight.StateMachine.TryRequestState<MeleeState>()) return true;
            return false;
        }

        public void ClearCollider()
        {
            _collisionDamageDealer.ClearEnemiesList();
            _collisionDamageDealer.GetComponent<Collider>().enabled = false;
        }
    }
}