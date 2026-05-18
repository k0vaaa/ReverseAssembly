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
            
            // Запускаем эффект глитча!
            if (EnemyAnimator != null)
            {
                EnemyAnimator.StartGlitchStun();
            }
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            if (NavMeshAgent != null && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = false;
            }
            
            // Выключаем эффект и возвращаем анимации в норму
            if (EnemyAnimator != null)
            {
                EnemyAnimator.StopGlitchStun();
            }
        }
    }
}
