using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Skills/SpellSkillData")]
    public class SpellSkillData : SkillData
    {
        public GameObject _projectilePrefab;
    }
}