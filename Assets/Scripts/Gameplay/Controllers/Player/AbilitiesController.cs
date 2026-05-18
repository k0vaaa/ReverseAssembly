using System;
using System.Collections.Generic;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class AbilitiesController : MonoBehaviour
    {
        [SerializeField] protected AbilitiesSet _abilitiesSet;
        [Inject] protected Container _container;
        protected Dictionary<Type,ISkill> _skills = new ();
        private Container _abilitiesContainer;

        public virtual void Init()
        {
            var wristController = GetComponent<WristTerminalController>();
            _abilitiesContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(wristController);
            });
            if (_abilitiesSet == null || _abilitiesSet.SkillEntries == null)
            {
                print($"No Skills on {gameObject.name}");
                return;
            }

            var context = new SkillContext()
            {
                FightController = GetComponent<FightController>(),
                Caster = gameObject,
                VFXController = GetComponent<VFXController>(),
                SFXController = GetComponent<SFXController>()
            };
            foreach (var skillDef in _abilitiesSet.SkillEntries)
            {
                var skill = skillDef.CreateSkill(_abilitiesContainer, context);
                if (!_skills.TryAdd(skill.GetType(), skill))
                {
                    Debug.LogWarning($"Error adding {skill.GetType().Name} to SkillsController on {gameObject.name}");
                }
            }
            print(String.Join(", ", _skills.Values.GetType().Name));
        }

        public T TryGetSkill<T>() where T : Skill
        {
            if (_skills.TryGetValue(typeof(T), out var skill))
            {
                return skill as T;
            }

            Debug.LogWarning($"Skill of type {typeof(T).Name} not found");
            return null;
        }
        
        public ISkill TryGetSkill(Type skillType)
        {
            if (_skills.TryGetValue(skillType, out var skill))
            {
                return skill;
            }

            Debug.LogWarning($"Skill of type {skillType.Name} not found");
            return null;
        }


        private void Update()
        {
            foreach (var skill in _skills)
            {
                skill.Value.Tick();
            }
        }

        private void OnDestroy()
        {
            _abilitiesContainer?.Dispose();
        }
    }
}