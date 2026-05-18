using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Definitions
{
    [CreateAssetMenu(fileName = "SwitchBranchDefinition", menuName = "Skills/SwitchBranch")]
    public class SwitchBranchAbilityDefinition : AbilityDefinition
    {
        public override Skill CreateSkill(Container container, SkillContext context)
        {
            var skill = new SwitchBranchSkill(this, context);
            AttributeInjector.Inject(skill, container);
            return skill;
        
        }
    }
}