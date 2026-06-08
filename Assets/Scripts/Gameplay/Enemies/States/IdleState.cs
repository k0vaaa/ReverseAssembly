using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class IdleState : StatesEnemyConst
    {
        public IdleState(AIController controller, EnemyAnimator animator, EnemyMover mover) : base(controller, animator, mover)
        {
        }

        protected override void EnterAction()
        {
            EnemyAnimator.IdleEvent();
            Mover.Stop();
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
            
        }
    }
}