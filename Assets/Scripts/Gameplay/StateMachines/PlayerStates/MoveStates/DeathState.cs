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

        protected override void EnterAction()
        {
            Debug.Log("Entering Death State");
            PlayerAnimator.DoDeath();
        }

        protected override void ExecuteAction()
        {
           
        }

        protected override void ExitAction()
        {
        }
    }
}
