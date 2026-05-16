using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.ScriptableObjects;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class SpellSkill : Skill
    {
        private readonly GameObject _spellProjectile;
        private readonly Transform _castPoint;
        private readonly Transform _caster;


        public SpellSkill(SkillData skillData,Transform castPoint, Transform caster) : base(skillData)
        {
            var _skillData = (SpellSkillData)skillData;
            _spellProjectile = _skillData._projectilePrefab;
            _castPoint = castPoint;
            _caster = caster;
        }

        protected override void CastAction()
        {
            var obj = Object.Instantiate(_spellProjectile,_castPoint.position, Quaternion.identity);
            var proj = obj.AddComponent<Projectile>();
            proj.Init(Damage, _castPoint.GetComponentInParent<IDamageable>());
            var rb = obj.GetComponent<Rigidbody>();
            int layerMask = ~LayerMask.GetMask(_castPoint.gameObject.tag);
            Physics.Raycast(_castPoint.position, _caster.forward, out RaycastHit hit, 1000f,layerMask);
            Vector3 castDir =  hit.point - _castPoint.position;
            rb.AddForce(castDir.normalized * 1000f);
            proj.StartCoroutine(proj.Ttl());
        }

        public void CastPlug()
        {
            base.TryCast();
        }
    }
}