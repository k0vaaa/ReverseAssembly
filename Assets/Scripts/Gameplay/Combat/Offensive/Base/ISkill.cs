namespace Gameplay.Combat.Offensive.Base
{
    public interface ISkill
    {
        public bool IsReady { get;}
        float GetReadyPercent();
        void TryCast();
        void Tick();
    }
}