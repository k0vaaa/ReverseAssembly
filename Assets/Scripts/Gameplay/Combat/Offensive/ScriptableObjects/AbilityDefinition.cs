using Gameplay.Combat.Offensive.Base;
using Reflex.Core;
using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    
    public abstract class AbilityDefinition : ScriptableObject
    {
        public string skillName;
        public float cooldownTime;
        public Sprite sprite;
        public Damage damage;

        public abstract Skill CreateSkill(Container container, SkillContext context);
    }
}