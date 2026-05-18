using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Definitions
{
    [CreateAssetMenu(fileName = "MeleeSkillDefinition", menuName = "Skills/MeleeSkill")]
    public class MeleeSkillDefinition : AbilityDefinition
    {
        public override Skill CreateSkill(Container container, SkillContext context)
        {
            var skill = new MeleeSkill(this, context);
            AttributeInjector.Inject(skill, container);
            skill.Init();
            return skill;
        
        }
    }
}