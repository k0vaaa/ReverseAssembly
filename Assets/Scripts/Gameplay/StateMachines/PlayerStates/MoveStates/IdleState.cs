using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class IdleState :  MovementPlayerState
    {
        public IdleState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        protected override void EnterAction()
        {
            PlayerAnimator.DoIdleMove();
            
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
            
        }
    }
}