using System.Collections;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class AttackState : StatesEnemyConst
    {
        private SkillsController _skillsController;
        public AttackState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent, SkillsController skillsController) : base(enemyController, animator, navMeshAgent)
        {
            _skillsController = skillsController;
        }

        public override void Enter()
        {
            _skillsController.Skills[SkillType.Melee].TryCast();
            EnemyAnimator.StartCoroutine(SwordColliderSwitch());
            Debug.Log("Entering ENEMY ATTACK");
            EnemyAnimator.DoAttack();
            NavMeshAgent.isStopped = true;
        }

        public override void Execute()
        {
            EnemyController.RotateToPlayer();
        }

        public override void Exit()
        {
            
        }
        
        private IEnumerator SwordColliderSwitch()
        {
            yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.33f, "attackTest"));
            EnemyController.SwordCollider.enabled = true;
            yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.63f, "attackTest"));
            EnemyController.SwordCollider.enabled = false;
        }
    }
}