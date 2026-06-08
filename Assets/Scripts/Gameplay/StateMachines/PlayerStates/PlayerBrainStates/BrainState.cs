using System;
using Core.StateMachines;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public abstract class BrainState : State
    {
        protected readonly FightController Fight;
        protected readonly PlayerBrain Brain;
        protected readonly MovementController Movement;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;

        protected BrainState(PlayerBrain brain, MovementController movement, FightController fight)
        {
            Brain = brain;
            Movement = movement;
            Fight = fight;
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