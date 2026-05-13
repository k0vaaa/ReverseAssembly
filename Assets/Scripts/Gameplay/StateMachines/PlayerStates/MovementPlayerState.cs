using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates
{
    public abstract class MovementPlayerState : IState
    {
        protected MovementController MovementController;
        protected IPlayerAnimator PlayerAnimator;
        
        protected MovementPlayerState(MovementController movementController, IPlayerAnimator animator)
        {
            MovementController = movementController;
            PlayerAnimator = animator;
        }
        
        public virtual void Enter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
        }
    }
}