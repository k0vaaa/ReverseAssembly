using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemySkillsController : AbilitiesController
    {
        public override void Init()
        {
            SkillsCreate();
        }
        private void SkillsCreate()
        {
            var context = new SkillContext()
            {
                EnemyController = GetComponent<EnemyController>(),
                Caster = gameObject
            };
            foreach (var skillDef in _abilitiesSet.SkillEntries)
            {
                var skill = skillDef.CreateSkill(_container, context);
                if (!_skills.TryAdd(skill.GetType(), skill))
                {
                    Debug.LogWarning($"Error adding {skill.GetType().Name} to SkillsController on {gameObject.name}");
                }
            }
        }
    }
}