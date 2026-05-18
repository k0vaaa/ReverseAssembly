using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Skills/SkillsArray", fileName = "SkillsArray")]
    public class AbilitiesSet : ScriptableObject
    {
        public List<AbilityDefinition> SkillEntries;
    }
}