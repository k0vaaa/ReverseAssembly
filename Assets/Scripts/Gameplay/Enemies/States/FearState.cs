using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class FearState : StatesEnemyConst
    {
        public FearState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent) : base(enemyController, animator, navMeshAgent)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering ENEMY Fear");
            EnemyAnimator.WalkEvent();
            if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = false;
            }
        }

        public override void Execute()
        {
            EnemyController.SetRunFromPlayer();
        }

        public override void Exit()
        {
            
        }
    }
}