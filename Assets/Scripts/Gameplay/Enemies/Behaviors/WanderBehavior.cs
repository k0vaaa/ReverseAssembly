using Gameplay.Enemies.States;
using UnityEngine;

namespace Gameplay.Enemies.Behaviors
{
    [CreateAssetMenu(menuName = "Enemies/Behaviors/Wander Behavior", fileName = "WanderBehavior")]
    public class WanderBehavior : EnemyBehaviorStrategy
    {
        [SerializeField] private float _wanderRadius = 10f;
        [SerializeField] private float _minWaitTime = 1f;
        [SerializeField] private float _maxWaitTime = 4f;

        public override void InitializeBehavior(EnemyBrain brain, AIController controller)
        {
            var stateMachine = brain.StateMachine;

            var enemyAnimator = controller.GetComponent<EnemyAnimator>();
            var mover = controller.GetComponent<EnemyMover>();
            var hpCanvas = controller.GetComponentInChildren<Canvas>();
            
            // Используем стартовую позицию в качестве якоря для блуждания
            Vector3 anchorPosition = controller.transform.position;

            var wanderState = new WanderState(controller, enemyAnimator, mover, _wanderRadius, _minWaitTime, _maxWaitTime, anchorPosition);
            var deathState = new DeathState(controller, enemyAnimator, mover, hpCanvas);
            var stunState = new GlitchStunState(controller, enemyAnimator, mover);

            controller.StabilitySystem.IsInvincible = true;
            stateMachine.AddAnyTransition(deathState, () => controller.StabilitySystem.Stability <= 0);
            
            stateMachine.AddState(wanderState);
            stateMachine.AddState(deathState);
            stateMachine.AddState(stunState);
            
            stateMachine.AddAnyTransition(stunState, () => brain.IsStunned);
            stateMachine.AddTransition(stunState, wanderState, () => !brain.IsStunned);

            stateMachine.TrySetState(wanderState);
        }
    }
}
