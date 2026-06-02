using Core.Gameplay;
using Core.StateMachines;
using Core.UI;

namespace Gameplay.StateMachines
{
    public abstract class GlobalState : IState
    {
        private GameplayManager _gameplayManager;
        private Window _window;

        protected GlobalState(GameplayManager gameplayManager, Window window)
        {
            _gameplayManager = gameplayManager;
            _window = window;
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