using System;
using Core.StateMachines;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public abstract class TerminalSubState : State
    {
        protected TerminalState _terminalState;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;

        protected TerminalSubState(TerminalState terminalState)
        {
            _terminalState = terminalState;
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