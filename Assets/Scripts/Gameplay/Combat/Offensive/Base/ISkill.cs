using System;

namespace Gameplay.Combat.Offensive.Base
{
    public interface ISkill
    {
        public bool IsReady { get;}
        float GetReadyPercent();
        bool TryCast();
        void Tick();
        public Action<float> OnCooldownTick { get; }
    }
}