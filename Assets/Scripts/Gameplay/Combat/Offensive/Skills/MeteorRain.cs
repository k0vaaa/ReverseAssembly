using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Collision;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Combat.Offensive.Skills.Definitions;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills
{
    public class MeteorRain : Skill
    {
        private readonly Transform _caster;
        private Transform _castPoint;
        private GameObject _prefab;
        private ProjectileAbilityDefinition _projectileAbilityDefinition;

        public MeteorRain(AbilityDefinition abilityDefinition,Transform castPoint, Transform caster, GameObject sword, IDamageable damageable) : base(abilityDefinition)
        {
            _caster = caster;
            _projectileAbilityDefinition = (ProjectileAbilityDefinition)abilityDefinition;
            _prefab = _projectileAbilityDefinition.ProjectilePrefab;
        }

        protected override void OnTick()
        {
            
        }

        protected override bool CastAction()
        {
            _castPoint = GameObject.FindGameObjectWithTag("Player").transform;
            var spell = Object.Instantiate(_prefab, _castPoint.position, Quaternion.identity).GetComponent<AOESpell>();
            spell.Init(_projectileAbilityDefinition.damage, _caster.GetComponent<IDamageable>());
            spell.StartCoroutine(spell.Ttl());
            return true;
        }
    }
}