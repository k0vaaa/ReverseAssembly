using UnityEngine;

namespace Gameplay.Enemies.BossStates
{
    public class BossWalkState : StatesBossConst
    {
        public BossWalkState(AIController controller, BossAnimator animator, EnemyMover mover) : base(controller, animator, mover)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering BOSS WALK");
            BossAnimator.WalkEvent();
            Mover.Resume();
        }

        protected override void ExecuteAction()
        {
            Mover.RotateToPlayer();
            Mover.SetFollowPlayer();
        }

        protected override void ExitAction()
        {
        }
    }
}