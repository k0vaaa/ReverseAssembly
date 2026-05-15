using System;
using Gameplay.Combat.Offensive.Base;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Combat.Offensive.ScriptableObjects
{
    [Serializable]
    public class SkillEntry
    {
        [SerializeField] private MonoScript _skillClass;
        [SerializeField] private SkillData _skillData;
        
        public SkillData SkillData => _skillData;
        public Type SkillClass
        {
            get
            {
                if (_skillClass != null && typeof(ISkill).IsAssignableFrom(_skillClass.GetClass()))
                {
                    return _skillClass.GetClass();
                }
                throw new ArgumentException("Invalid skill script");
            }
        }
    }
}