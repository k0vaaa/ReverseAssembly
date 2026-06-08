using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.States;
using UnityEngine;

namespace Gameplay.Enemies.Behaviors
{
    [CreateAssetMenu(menuName = "Enemies/Behaviors/Ranged Behavior", fileName = "RangedBehavior")]
    public class RangedBehavior : EnemyBehaviorStrategy
    {
        public override void InitializeBehavior(EnemyBrain brain, AIController controller)
        {
            var stateMachine = brain.StateMachine;
            
            var enemyAnimator = controller.GetComponent<EnemyAnimator>();
            var mover = controller.GetComponent<EnemyMover>();
            var abilities = controller.GetComponent<AbilitiesController>();
            var hpCanvas = controller.GetComponentInChildren<Canvas>();

            var attackState = new RangeAttackState(controller, enemyAnimator, mover, abilities);
            bool AttackReady() => abilities.TryGetSkill<ProjectileSkill>() is { IsReady: true };

            var idleState = new IdleState(controller, enemyAnimator, mover);
            var walkState = new WalkState(controller, enemyAnimator, mover);
            var fearState = new FearState(controller, enemyAnimator, mover);
            var deathState = new DeathState(controller, enemyAnimator, mover, hpCanvas);
            var stunState = new GlitchStunState(controller, enemyAnimator, mover);

            stateMachine.AddAnyTransition(deathState, () => controller.StabilitySystem.Stability <= 0f);
            
            stateMachine.AddState(idleState);
            stateMachine.AddState(walkState);
            stateMachine.AddState(fearState);
            stateMachine.AddState(deathState);
            stateMachine.AddState(attackState);
            stateMachine.AddState(stunState);
            
            stateMachine.AddAnyTransition(stunState, () => brain.IsStunned);
            stateMachine.AddTransition(stunState, idleState, () => !brain.IsStunned);

            stateMachine.AddTransition(idleState, fearState, 
                () => brain.IsPeaceful && (controller.StabilitySystem.Stability / controller.StabilitySystem.MaxStability <= 0.3f));

            stateMachine.AddTransition(idleState, walkState, () => !brain.IsPeaceful && brain.IsChasing && !brain.IsInAttackRange);
            stateMachine.AddTransition(idleState, attackState, () => !brain.IsPeaceful && brain.IsInAttackRange && AttackReady());

            stateMachine.AddTransition(walkState, idleState, () => brain.IsPeaceful || !brain.IsChasing || brain.IsInAttackRange);
            stateMachine.AddTransition(walkState, attackState, () => !brain.IsPeaceful && brain.IsInAttackRange && AttackReady());
            
            bool AttackAnimEnded() => enemyAnimator.CheckAnimationState(0, 0.99f, "RangeAttack");
            
            stateMachine.AddTransition(attackState, idleState, () => brain.IsInAttackRange && AttackAnimEnded());
            stateMachine.AddTransition(attackState, walkState, () => !brain.IsInAttackRange && AttackAnimEnded());

            stateMachine.TrySetState(idleState);
        }
    }
}
