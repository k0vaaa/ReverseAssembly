using System;
using Core.Gameplay;
using Core.StateMachines;
using Core.UI;

namespace Gameplay.StateMachines
{
    public abstract class GlobalState : State
    {
        private GameplayManager _gameplayManager;
        private Window _window;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;

        protected GlobalState(GameplayManager gameplayManager, Window window)
        {
            _gameplayManager = gameplayManager;
            _window = window;
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