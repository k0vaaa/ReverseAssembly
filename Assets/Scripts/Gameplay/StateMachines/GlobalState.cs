using Core.Gameplay;
using Core.UI;

namespace Gameplay.StateMachines
{
    public abstract class GlobalState : IState
    {
        private GameplayManager _gameplayManager;
        private ViewManager _viewManager;

        protected GlobalState(GameplayManager gameplayManager, ViewManager viewManager)
        {
            _gameplayManager = gameplayManager;
            _viewManager = viewManager;
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