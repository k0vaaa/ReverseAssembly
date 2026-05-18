using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Definitions
{
    [CreateAssetMenu(fileName = "ProjectileSkillDefinition", menuName = "Skills/ProjectileSkill")]
    public class ProjectileAbilityDefinition : AbilityDefinition
    {
        public GameObject ProjectilePrefab;
        public float Velocity;

        public override Skill CreateSkill(Container container, SkillContext context)
        {
            var skill = new ProjectileSkill(this,context);
            AttributeInjector.Inject(skill,container);
            return skill;
        }
    }
}