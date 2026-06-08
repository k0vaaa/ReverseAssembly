using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Enemies.BossStates
{
    public class BossSuperAttackState : StatesBossConst
    {
        private AbilitiesController _abilitiesController;

        public BossSuperAttackState(AIController controller, BossAnimator animator, EnemyMover mover, AbilitiesController abilitiesController) : base(controller, animator, mover)
        {
            _abilitiesController = abilitiesController;
        }

        protected override void EnterAction()
        {
            _abilitiesController.TryGetSkill<HeavyAttack>().TryCast();
            Debug.Log("Entering BOSS SUPER ATTACK");
            Mover.Stop();
            BossAnimator.DoSuperAttack();
        }

        protected override void ExecuteAction()
        {
            Mover.RotateToPlayer();
        }

        protected override void ExitAction()
        {
        }
    }
}