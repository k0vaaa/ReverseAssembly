using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class FallingState : MovementPlayerState
    {
        public FallingState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering Falling State");
            PlayerAnimator.DoFalling();
        }

        protected override void ExecuteAction()
        {
            MovementController.InertialMove();
        }

        protected override void ExitAction()
        {
            Debug.Log("Exiting Falling State");
            PlayerAnimator.DoLanding();
        }
    }
}
