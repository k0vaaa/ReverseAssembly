using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossAttackState : StatesBossConst
    {
        private AbilitiesController _abilitiesController;
        public BossAttackState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent, AbilitiesController abilitiesController) : base(bossController, animator, navMeshAgent)
        {
            _abilitiesController = abilitiesController;
        }

        public override void Enter()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().TryCast();
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
