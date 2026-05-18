using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossSuperAttackState : StatesBossConst
    {
        private AbilitiesController _abilitiesController;
        public BossSuperAttackState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent, AbilitiesController abilitiesController) : base(bossController, animator, navMeshAgent)
        {
            _abilitiesController = abilitiesController;
        }

        public override void Enter()
        {
            _abilitiesController.TryGetSkill<HeavyAttack>().TryCast();
            BossAnimator.DoSuperAttack();
            NavMeshAgent.isStopped = true;
            Debug.Log("Entering BOSS SUPER ATTACK");
        }

        public override void Execute()
        {
            BossController.RotateToPlayer();
        }

        public override void Exit()
        {
            Debug.Log("exit");
        }
    }
}