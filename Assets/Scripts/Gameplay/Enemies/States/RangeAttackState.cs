using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class RangeAttackState : StatesEnemyConst
    {
        private readonly AbilitiesController _abilitiesController;

        public RangeAttackState(AIController controller, EnemyAnimator animator, EnemyMover mover, AbilitiesController abilitiesController) : base(controller, animator, mover)
        {
            _abilitiesController = abilitiesController;
        }

        protected override void EnterAction()
        {
            _abilitiesController.TryGetSkill<ProjectileSkill>().TryCast();
            Debug.Log("Entering ENEMY Range Attack");
            Mover.Stop();
            
            if (Controller.TryGetComponent(out EnemyAnimator anim))
            {
                anim.DoAttack();
            }
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
