using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class WalkState : StatesEnemyConst
    {
        public WalkState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent) : base(enemyController, animator, navMeshAgent)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering ENEMY WALK");
            EnemyAnimator.WalkEvent();
            if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = false;
            }
        }

        public override void Execute()
        {
            EnemyController.SetFollowPlayer();
        }

        public override void Exit()
        {
            
        }
    }
}