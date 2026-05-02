namespace Gameplay.Combat.Offensive.Base
{
    public interface ISkill
    {
        public bool _isReady { get;}
        float GetReadyPercent();
        void Cast();
        void Tick();
    }
}