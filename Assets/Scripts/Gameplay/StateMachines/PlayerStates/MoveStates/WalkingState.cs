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

        public override void Enter()
        {
            MonoBehaviour.print("Entering Walking State");
            PlayerAnimator.DoWalk();
            MovementController.CurrentMoveSpeed = MovementController.WalkSpeed;
        }

        public override void Execute()
        {
            MovementController.Move();
        }

        public override void Exit()
        {
            MonoBehaviour.print("Exiting Walking State");
            
        }
    }
}
