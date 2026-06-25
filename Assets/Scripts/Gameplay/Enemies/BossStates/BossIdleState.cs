using UnityEngine;

namespace Gameplay.Enemies.BossStates
{
    public class BossIdleState : StatesBossConst
    {
        public BossIdleState(AIController controller, BossAnimator animator, EnemyMover mover) : base(controller, animator, mover)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering BOSS Idle");
            BossAnimator.IdleEvent();
            Mover.Stop();
        }

        protected override void ExecuteAction()
        {
            Mover.RotateToPlayer();
        }

        protected override void ExitAction()
        {
        }
    }
}