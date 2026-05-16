using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossSuperAttackState : StatesBossConst
    {
        private SkillsController _skillsController;
        public BossSuperAttackState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent, SkillsController skillsController) : base(bossController, animator, navMeshAgent)
        {
            _skillsController = skillsController;
        }

        public override void Enter()
        {
            _skillsController.Skills[SkillType.Heavy].TryCast();
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