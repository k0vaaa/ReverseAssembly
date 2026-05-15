using System.Collections;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class RangeAttackState : StatesEnemyConst
    {
        
        private SkillsController _skillsController;
        public RangeAttackState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent, SkillsController skillsController ) : base(enemyController, animator, navMeshAgent)
        {
            _skillsController = skillsController;
        }

        public override void Enter()
        {
            Debug.Log("Entering ENEMY RANGE ATTACK");
            EnemyAnimator.DoAttack();
            NavMeshAgent.isStopped = true;
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
            var plug = (SpellSkill)_skillsController.Skills[SkillType.Fireball];
            if (Random.Range(0, 2) == 1)
            {
                _skillsController.Skills[SkillType.Fireball].Cast();
            }
            else
            {
                if (_skillsController.Skills[SkillType.Meteor]._isReady)
                {
                    _skillsController.Skills[SkillType.Meteor].Cast();
                    plug.CastPlug();
                }
                else
                {
                    _skillsController.Skills[SkillType.Fireball].Cast();
                }
            }
            
        }
    }
}