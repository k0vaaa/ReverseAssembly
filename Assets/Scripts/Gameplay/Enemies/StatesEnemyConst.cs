using Core.StateMachines;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public abstract class StatesEnemyConst : IState
    {
        protected EnemyController EnemyController;
        protected EnemyAnimator EnemyAnimator;
        protected NavMeshAgent NavMeshAgent;
  
        
        protected StatesEnemyConst(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent)
        {
            EnemyController = enemyController;
            EnemyAnimator = animator;
            NavMeshAgent = navMeshAgent;
        }
        
        public virtual void Enter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
        }
    }
}