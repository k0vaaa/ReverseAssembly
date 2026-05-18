using Core.StateMachines;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public abstract class StatesBossConst : IState
    {
        protected BossController BossController;
        protected BossAnimator BossAnimator;
        protected NavMeshAgent NavMeshAgent;
        
        protected StatesBossConst(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent)
        {
            BossController = bossController;
            BossAnimator = animator;
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