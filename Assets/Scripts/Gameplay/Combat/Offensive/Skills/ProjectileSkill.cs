using Core.Extensions;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.Helpers;
using Gameplay.Combat.Offensive.Skills.Definitions;
using Gameplay.StateMachines.PlayerStates.FightStates;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class ProjectileSkill : Skill
    {
        [Inject] private Camera _camera;
        private readonly ProjectileAbilityDefinition _def;
        private readonly SkillContext _ctx;
        private Transform _castPoint;
        private Transform _caster;

        public ProjectileSkill(ProjectileAbilityDefinition abilityDefinition,SkillContext ctx) : base(abilityDefinition)
        {
            _def = abilityDefinition;
            _ctx = ctx;
            _caster = _ctx.Caster.transform;
            _castPoint = _caster.GetComponentInChildren<CastPoint>(true).transform;
        }

        protected override void OnTick()
        {
            
        }

        protected override bool CastAction()
        {
            if (!_ctx.FightController.StateMachine.TryRequestState<RangedSkillState>()) return false;
            var obj = Object.Instantiate(_def.ProjectilePrefab,_castPoint.position, Quaternion.identity);
            
            var proj = obj.AddComponent<Projectile>();
            proj.Init(Damage, _castPoint.GetComponentInParent<IDamageable>());
            
            var rb = obj.GetComponent<Rigidbody>();
            int layerMask = ~_castPoint.gameObject.layer;

            Vector3 castDir;
            if (Physics.Raycast(_castPoint.position, _castPoint.forward, out RaycastHit hit, 1000f,layerMask))
            {
                 castDir =  hit.point - _castPoint.position;
            }
            else
            {
                Ray ray = new Ray( _caster.position.WithY(_castPoint.position.y), _camera.transform.forward);
                
                castDir = ray.GetPoint(300) - _castPoint.position;
            }
            
            rb.AddForce(castDir.normalized * _def.Velocity, ForceMode.VelocityChange);
            
            proj.StartCoroutine(proj.Ttl());
            return true;
        }

        public void CastPlug()
        {
            base.TryCast();
        }
    }
}