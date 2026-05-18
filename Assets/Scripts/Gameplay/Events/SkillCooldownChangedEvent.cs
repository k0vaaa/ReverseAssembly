using Core.Events;
using Gameplay.Combat.Offensive.Base;

namespace Gameplay.Events
{
    public struct SkillCooldownChangedEvent : IEvent
    {
        public SkillType SkillType;
        public float ReadyPercent;
    }
}