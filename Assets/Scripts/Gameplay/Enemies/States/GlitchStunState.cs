using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class GlitchStunState : StatesEnemyConst
    {
        public GlitchStunState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent) 
            : base(enemyController, animator, navMeshAgent) { }

        public override void Enter()
        {
            if (NavMeshAgent != null && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = true;
            }
            if (EnemyAnimator != null && EnemyAnimator._animator != null)
            {
                EnemyAnimator._animator.speed = 0f;
            }
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            if (EnemyAnimator != null && EnemyAnimator._animator != null)
            {
                EnemyAnimator._animator.speed = 1f;
            }
            if (NavMeshAgent != null && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = false;
            }
        }
    }
}
