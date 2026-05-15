using Gameplay.Combat.Offensive.Base;
using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Skills/SkillData")]
    public class SkillData : ScriptableObject
    {
        public SkillType skillType;
        public string skillName;
        public float cooldownTime;
        public Sprite sprite;
        public Damage damage;
    }
}