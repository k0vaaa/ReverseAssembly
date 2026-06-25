using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Enemies.BossStates
{
    public class BossAttackState : StatesBossConst
    {
        private AbilitiesController _abilitiesController;

        public BossAttackState(AIController controller, BossAnimator animator, EnemyMover mover, AbilitiesController abilitiesController) : base(controller, animator, mover)
        {
            _abilitiesController = abilitiesController;
        }

        protected override void EnterAction()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().TryCast();
            Debug.Log("Entering BOSS ATTACK");
            Mover.Stop();
            BossAnimator.DoAttack();
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
