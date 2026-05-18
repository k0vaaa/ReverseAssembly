using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Definitions
{
    [CreateAssetMenu(fileName = "ScannerSkillDefinition", menuName = "Skills/ScannerSkill")]
    public class ScannerAbilityDefinition : AbilityDefinition
    {
        public float ScanDistance = 10f;
        public LayerMask InteractableLayer;
        public override Skill CreateSkill(Container container, SkillContext context)
        {
            var skill = new ScannerSkill(this, context);
            AttributeInjector.Inject(skill, container);
            skill.Init();
            return skill;
        }
    }
}