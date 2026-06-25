using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class SprintingState : MovementPlayerState
    {
        public SprintingState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        protected override void EnterAction()
        {
            PlayerAnimator.DoRun();
            MovementController.CurrentMoveSpeed = MovementController.RunSpeed;
            MonoBehaviour.print("Entering Sprinting State");
        }

        protected override void ExecuteAction()
        {
            MovementController.Move();
        }

        protected override void ExitAction()
        {
            MonoBehaviour.print("Exiting Sprinting State");
        }
    }
}