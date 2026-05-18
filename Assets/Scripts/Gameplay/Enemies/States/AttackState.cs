using System.Collections;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class AttackState : StatesEnemyConst
    {
        private readonly EnemySkillsController _abilitiesController;

        public AttackState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent, EnemySkillsController abilitiesController) : base(enemyController, animator, navMeshAgent)
        {
            _abilitiesController = abilitiesController;
        }

        public override void Enter()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().TryCast();
            EnemyController.ResetAnim();
            Debug.Log("Entering ENEMY ATTACK");
            EnemyAnimator.DoAttack();
            if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = true;
            }
        }

        public override void Execute()
        {
            EnemyController.RotateToPlayer();
        }

        public override void Exit()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().ClearCollider();
        }
        
    }
}
