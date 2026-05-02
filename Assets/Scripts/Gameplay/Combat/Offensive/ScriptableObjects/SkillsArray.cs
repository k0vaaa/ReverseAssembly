using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Skills/SkillsArray", fileName = "SkillsArray")]
    public class SkillsArray : ScriptableObject
    {
        public SkillEntry[] skillEntries;
    }
}