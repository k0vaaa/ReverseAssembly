using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossWalkState : StatesBossConst
    {
        private AbilitiesController _abilitiesController;
        public BossWalkState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent) : base(bossController, animator, navMeshAgent)
        {
            // _skillsController = skillsController;
        }

        public override void Enter()
        {
            Debug.Log("Entering BOSS WALK");
            BossAnimator.WalkEvent();
            NavMeshAgent.isStopped = false;
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
        //     yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.3f, "attackTest"));
        //     EnemyController.SwordCollider.enabled = true;
        //     yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.53f, "attackTest"));
        //     EnemyController.SwordCollider.enabled = false;
        // }
    }
}