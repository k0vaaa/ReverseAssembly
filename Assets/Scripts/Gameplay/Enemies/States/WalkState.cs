using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class WalkState : StatesEnemyConst
    {
        public WalkState(AIController controller, EnemyAnimator animator, EnemyMover mover) : base(controller, animator, mover)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering ENEMY WALK");
            EnemyAnimator.WalkEvent();
            Mover.Resume();
        }

        protected override void ExecuteAction()
        {
            Mover.SetFollowPlayer();
        }

        protected override void ExitAction()
        {
            
        }
    }
}