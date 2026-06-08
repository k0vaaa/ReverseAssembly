using Gameplay.Anims;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.States;
using UnityEngine;

namespace Gameplay.Enemies.Behaviors
{
    [CreateAssetMenu(menuName = "Enemies/Behaviors/Melee Behavior", fileName = "MeleeBehavior")]
    public class MeleeBehavior : EnemyBehaviorStrategy
    {
        private bool _attackAnimEnded;

        public override void InitializeBehavior(EnemyBrain brain, AIController controller)
        {
            var stateMachine = brain.StateMachine;

            var enemyAnimator = controller.GetComponent<EnemyAnimator>();
            var mover = controller.GetComponent<EnemyMover>();
            var abilities = controller.GetComponent<AbilitiesController>();
            var hpCanvas = controller.GetComponentInChildren<Canvas>();
            var animEventsHandler = controller.GetComponent<AnimationEventsHandler>();

            var attackState = new AttackState(controller, enemyAnimator, mover, abilities);
            bool AttackReady() => abilities.TryGetSkill<PunchSkill>() is { IsReady: true };
            attackState.OnEnter += ResetAttackAnim;
            animEventsHandler.OnAnimationEnded += HandleAnimEnd;

            var idleState = new IdleState(controller, enemyAnimator, mover);
            var walkState = new WalkState(controller, enemyAnimator, mover);
            var fearState = new FearState(controller, enemyAnimator, mover);
            var deathState = new DeathState(controller, enemyAnimator, mover, hpCanvas);
            var stunState = new GlitchStunState(controller, enemyAnimator, mover);

            stateMachine.AddAnyTransition(deathState, () => controller.StabilitySystem.Stability <= 0);
            
            stateMachine.AddState(idleState);
            stateMachine.AddState(walkState);
            stateMachine.AddState(fearState);
            stateMachine.AddState(deathState);
            stateMachine.AddState(attackState);
            stateMachine.AddState(stunState);
            
            stateMachine.AddAnyTransition(stunState, () => brain.IsStunned);
            stateMachine.AddTransition(stunState, idleState, () => !brain.IsStunned);

            // Peaceful transitions (handled by brain's peaceful property implicitly but we can add specific logic here if needed)
            // Wait, previously we added FearState when peaceful. BranchAggroHandler toggles IsPeaceful.
            stateMachine.AddTransition(idleState, fearState, 
                () => brain.IsPeaceful && (controller.StabilitySystem.Stability / controller.StabilitySystem.MaxStability <= 0.3f));

            // Aggressive transitions
            stateMachine.AddTransition(idleState, walkState, () => !brain.IsPeaceful && brain.IsChasing && !brain.IsInAttackRange);
            stateMachine.AddTransition(idleState, attackState, () => !brain.IsPeaceful && brain.IsInAttackRange && AttackReady());

            stateMachine.AddTransition(walkState, idleState, () => brain.IsPeaceful || !brain.IsChasing || brain.IsInAttackRange);
            stateMachine.AddTransition(walkState, attackState, () => !brain.IsPeaceful && brain.IsInAttackRange && AttackReady());

            
            stateMachine.AddTransition(attackState, idleState, () => brain.IsInAttackRange && _attackAnimEnded);
            stateMachine.AddTransition(attackState, walkState, () => !brain.IsInAttackRange && _attackAnimEnded);

            stateMachine.TrySetState(idleState);
        }

        private void HandleAnimEnd()
        {
            _attackAnimEnded = true;
        }

        private void ResetAttackAnim()
        {
            _attackAnimEnded = false;
        }
    }
}
