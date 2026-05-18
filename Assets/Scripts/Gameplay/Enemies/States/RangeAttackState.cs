using System.Collections;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class RangeAttackState : StatesEnemyConst
    {
        
        private AbilitiesController _abilitiesController;
        public RangeAttackState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent, AbilitiesController abilitiesController ) : base(enemyController, animator, navMeshAgent)
        {
            _abilitiesController = abilitiesController;
        }

        public override void Enter()
        {
            Debug.Log("Entering ENEMY RANGE ATTACK");
            EnemyAnimator.DoAttack();
            if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = true;
            }
            EnemyAnimator.StartCoroutine(SpellCast());
        }

        public override void Execute()
        {
            EnemyController.RotateToPlayer();
        }

        public override void Exit()
        {
            
        }
        
        private IEnumerator SpellCast()
        {
            yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.425f, "attackTest"));
            var plug = _abilitiesController.TryGetSkill<ProjectileSkill>();
            if (Random.Range(0, 2) == 1)
            {
                _abilitiesController.TryGetSkill<ProjectileSkill>().TryCast();
            }
            else
            {
                if (_abilitiesController.TryGetSkill<MeteorRain>().IsReady)
                {
                    _abilitiesController.TryGetSkill<MeteorRain>().TryCast();
                    plug.CastPlug();
                }
                else
                {
                    _abilitiesController.TryGetSkill<ProjectileSkill>().TryCast();
                }
            }
            
        }
    }
}
