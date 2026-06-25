using System;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public abstract class MovementPlayerState : State
    {
        protected MovementController MovementController;
        protected IPlayerAnimator PlayerAnimator;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;

        protected MovementPlayerState(MovementController movementController, IPlayerAnimator animator)
        {
            MovementController = movementController;
            PlayerAnimator = animator;
        }
        
        protected override void EnterAction()
        {
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
        }

        public Action OnEnter
        {
            get => _onEnter;
            set => _onEnter = value;
        }

        public Action OnExecute
        {
            get => _onExecute;
            set => _onExecute = value;
        }

        public Action OnExit
        {
            get => _onExit;
            set => _onExit = value;
        }
    }
}