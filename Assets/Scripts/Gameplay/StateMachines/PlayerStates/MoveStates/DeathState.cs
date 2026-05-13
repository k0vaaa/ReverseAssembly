using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class DeathState : MovementPlayerState
    {
        public DeathState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering Death State");
            PlayerAnimator.DoDeath();
        }

        public override void Execute()
        {
           
        }

        public override void Exit()
        {
        }
    }
}
