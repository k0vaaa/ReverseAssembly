using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.BossStates;
using UnityEngine;

namespace Gameplay.Enemies.Behaviors
{
    [CreateAssetMenu(menuName = "Enemies/Behaviors/Boss Behavior", fileName = "BossBehavior")]
    public class BossBehavior : EnemyBehaviorStrategy
    {
        public override void InitializeBehavior(EnemyBrain brain, AIController controller)
        {
            var stateMachine = brain.StateMachine;
            
            var bossAnimator = controller.GetComponent<BossAnimator>();
            var mover = controller.GetComponent<EnemyMover>();
            var abilities = controller.GetComponent<AbilitiesController>();
            var hpCanvas = controller.GetComponentInChildren<Canvas>();

            var attackState = new BossAttackState(controller, bossAnimator, mover, abilities);
            bool AttackReady() => abilities.TryGetSkill<PunchSkill>() is { IsReady: true };
            bool HeavyAttackReady() => abilities.TryGetSkill<HeavyAttack>() is { IsReady: true };

            var superAttackState = new BossSuperAttackState(controller, bossAnimator, mover, abilities);
            
            var idleState = new BossIdleState(controller, bossAnimator, mover);
            var walkState = new BossWalkState(controller, bossAnimator, mover);
            var deathState = new BossDeathState(controller, bossAnimator, mover, hpCanvas);
            
            bool AttackAnimationEnded() => bossAnimator.CheckAnimationState(0, .99f, "BossAttack");
            bool HeavyAttackAnimationEnded() => bossAnimator.CheckAnimationState(0, .99f, "BossSuperAttack");
            bool WasHit() => !Mathf.Approximately(controller.StabilitySystem.Stability, controller.StabilitySystem.MaxStability);

            stateMachine.AddAnyTransition(deathState, () => controller.StabilitySystem.Stability <= 0f);
            
            stateMachine.AddState(idleState);
            stateMachine.AddState(walkState);
            stateMachine.AddState(deathState);
            stateMachine.AddState(attackState);
            stateMachine.AddState(superAttackState);

            stateMachine.AddTransition(idleState, walkState, () => brain.IsChasing && !brain.IsInAttackRange && (!brain.IsPeaceful || WasHit()));
            stateMachine.AddTransition(idleState, superAttackState,
                () => brain.IsInAttackRange && HeavyAttackReady() && (!brain.IsPeaceful || WasHit()));
            stateMachine.AddTransition(idleState, attackState,
                () => brain.IsInAttackRange && AttackReady() && !HeavyAttackReady() && (!brain.IsPeaceful || WasHit()));
            
            stateMachine.AddTransition(walkState, superAttackState, () => brain.IsInAttackRange && HeavyAttackReady());
            stateMachine.AddTransition(walkState, attackState, () => brain.IsInAttackRange && AttackReady() && !HeavyAttackReady());
            stateMachine.AddTransition(walkState, idleState, () => !brain.IsChasing || brain.IsInAttackRange);
            
            stateMachine.AddTransition(attackState, idleState, () => brain.IsInAttackRange && AttackAnimationEnded());
            stateMachine.AddTransition(attackState, superAttackState, () => brain.IsInAttackRange && AttackAnimationEnded() && HeavyAttackReady());
            stateMachine.AddTransition(attackState, walkState, () => !brain.IsInAttackRange && AttackAnimationEnded());

            stateMachine.AddTransition(superAttackState, idleState, () => brain.IsInAttackRange && HeavyAttackAnimationEnded());
            stateMachine.AddTransition(superAttackState, walkState, () => !brain.IsInAttackRange && HeavyAttackAnimationEnded());

            stateMachine.TrySetState(idleState);
        }
    }
}
