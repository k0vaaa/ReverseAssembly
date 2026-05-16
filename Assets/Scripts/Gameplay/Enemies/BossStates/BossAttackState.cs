using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossAttackState : StatesBossConst
    {
        private SkillsController _skillsController;
        public BossAttackState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent, SkillsController skillsController) : base(bossController, animator, navMeshAgent)
        {
            _skillsController = skillsController;
        }

        public override void Enter()
        {
            _skillsController.Skills[SkillType.Punch].TryCast();
            // EnemyAnimator.StartCoroutine(SwordColliderSwitch());
            Debug.Log("Entering BOSS ATTACK");
            BossAnimator.DoAttack();
            NavMeshAgent.isStopped = true;
        }

        public override void Execute()
        {
            BossController.RotateToPlayer();
        }

        public override void Exit()
        {
            
        }
        
        // private IEnumerator SwordColliderSwitch()
        // {
            // yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.3f, "attackTest"));
            // EnemyController.SwordCollider.enabled = true;
            // yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.53f, "attackTest"));
            // EnemyController.SwordCollider.enabled = false;
        // }
    }
}