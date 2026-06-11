using System;
using Core.Extensions;
using Core.Utilities;

namespace Core.StateMachines
{
    public abstract class State : IState
    {
        public void Enter()
        {
            //Logger.LogTyped(GetType(),"Enter");
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
            //Logger.LogTyped(GetType(),"Exit");
            
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