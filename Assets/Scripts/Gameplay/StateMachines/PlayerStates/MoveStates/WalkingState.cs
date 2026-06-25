using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class WalkingState: MovementPlayerState
    {
        public WalkingState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        protected override void EnterAction()
        {
            
            PlayerAnimator.DoWalk();
            MovementController.CurrentMoveSpeed = MovementController.WalkSpeed;
        }

        protected override void ExecuteAction()
        {
            MovementController.Move();
        }

        protected override void ExitAction()
        {
            
            
        }
    }
}
