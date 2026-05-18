using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Definitions
{
    [CreateAssetMenu(fileName = "PunchSkillDefinition", menuName = "Skills/PunchSkill")]
    public class PunchSkillDefinition : AbilityDefinition
    {
        public override Skill CreateSkill(Container container, SkillContext context)
        {
            var skill = new PunchSkill(this, context);
            AttributeInjector.Inject(skill, container);
            skill.Init();
            return skill;
        
        }
    }
}