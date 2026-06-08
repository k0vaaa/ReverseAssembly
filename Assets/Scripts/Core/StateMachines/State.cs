using System;

namespace Core.StateMachines
{
    public abstract class State : IState
    {
        public void Enter()
        {
            EnterAction();
            OnEnter?.Invoke();
        }

        public void Execute()
        {
            ExecuteAction();
            OnExecute?.Invoke();
        }

        public void Exit()
        {
            ExitAction();
            OnExit?.Invoke();
        }

        protected virtual void EnterAction()
        {
            
        }

        protected virtual void ExecuteAction()
        {
            
        }

        protected virtual void ExitAction()
        {
            
        }

        public event Action OnEnter;

        public event Action OnExecute;

        public event Action OnExit;
    }
}