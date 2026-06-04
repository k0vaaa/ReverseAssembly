using Core.StateMachines;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public abstract class TerminalSubState : IState
    {
        protected TerminalState _terminalState;

        protected TerminalSubState(TerminalState terminalState)
        {
            _terminalState = terminalState;
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