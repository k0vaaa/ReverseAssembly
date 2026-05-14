using System;
using System.Collections.Generic;
using Core.DI;
using Gameplay.Abilities;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class SkillsController : MonoBehaviour, IInjectable
    {
        [SerializeField] private SkillsArray _skillsArray;
        [SerializeField] private Transform _castPoint;
        [SerializeField] private Transform _caster;
        [SerializeField] public GameObject _sword;

        [Inject] private BranchManager _branchManager;

        private Camera _camera;
        public Dictionary<SkillType,ISkill> Skills = new ();

        public void Awake()
        {
            /*var spell = new SpellSkill(_skillsArray.skillEntries[1],_castPoint, Camera.main.transform);
            var melee = new MeleeSkill(_skillsArray.skillEntries[0]);
            Skills.Add(spell.SkillType,spell);
            Skills.Add(melee.SkillType,melee);*/
            
            
        }

        public void Init(Camera camera)
        {
            if (camera)
            {
                _camera = camera;
                _caster = _camera.transform;
            }
            else
            {
                _caster = transform;
            }

            if (_skillsArray == null || _skillsArray.skillEntries == null) return;

            foreach (var skillEntry in _skillsArray.skillEntries)
            {
                if (skillEntry.SkillClass == typeof(SwitchBranchAbility))
                {
                    var skill = new SwitchBranchAbility(skillEntry.SkillData, _branchManager);
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(ScannerAbility))
                {
                    var skill = new ScannerAbility(skillEntry.SkillData);
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(MeleeSkill))
                {
                    var skill = new MeleeSkill(skillEntry.SkillData, _sword, gameObject.GetComponent<IDamageable>());
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(SpellSkill))
                {
                    var skill = new SpellSkill(skillEntry.SkillData, _castPoint, _caster);
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(PunchSkill))
                {
                    var skill = new PunchSkill(skillEntry.SkillData, _castPoint, _caster, _sword, gameObject.GetComponent<IDamageable>());
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(MeteorRain))
                {
                    var skill = new MeteorRain(skillEntry.SkillData, _castPoint, _caster, _sword, gameObject.GetComponent<IDamageable>());
                    Skills[skill.SkillType] = skill;
                }
                else if (skillEntry.SkillClass == typeof(HeavyAttack))
                {
                    var skill = new HeavyAttack(skillEntry.SkillData, _castPoint, _caster, _sword, gameObject.GetComponent<IDamageable>());
                    Skills[skill.SkillType] = skill;
                }
            }
        }

        private void Update()
        {
            foreach (var skill in Skills)
            {
                skill.Value.Tick();
            }
        }
    }
}