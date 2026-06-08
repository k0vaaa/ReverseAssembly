using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class AttackState : StatesEnemyConst
    {
        private readonly AbilitiesController _abilitiesController;

        public AttackState(AIController controller, EnemyAnimator animator, EnemyMover mover, AbilitiesController abilitiesController) : base(controller, animator, mover)
        {
            _abilitiesController = abilitiesController;
        }

        protected  override void EnterAction()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().TryCast();
            Debug.Log("Entering ENEMY Attack");
            Mover.Stop();
            
            // To trigger events from animations
            if (Controller.TryGetComponent(out EnemyAnimator anim))
            {
                anim.DoAttack();
            }
        }

        protected  override void ExecuteAction()
        {
            Mover.RotateToPlayer();
        }

        protected  override void ExitAction()
        {
            _abilitiesController.TryGetSkill<PunchSkill>().ClearCollider();
        }
    }
}
