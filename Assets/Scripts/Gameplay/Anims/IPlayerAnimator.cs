namespace Gameplay.Anims
{
    public interface IPlayerAnimator
    {
        bool CheckAnimationState(int layerIndex, float time, string stateName);
        void DoAttack();
        void DoSheath();
        void DoWithdraw();
        void DoSpell();
        void DoHit();
        void DoJump();
        void DoWalk();
        void DoRun();
        void DoIdleMove();
        void DoFalling();
        void DoLanding();
        void DoDeath();
    }
}
