using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class FearState : EnemyState
    {
        public FearState(AIController controller, EnemyAnimator animator, EnemyMover mover) : base(controller, animator, mover)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering ENEMY Fear");
            EnemyAnimator.WalkEvent();
            Mover.Resume();
        }
        
    }
}