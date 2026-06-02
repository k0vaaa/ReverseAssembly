using Core.StateMachines;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public abstract class BrainState : IState
    {
        protected readonly FightController Fight;
        protected readonly PlayerBrain Brain;
        protected readonly MovementController Movement;

        protected BrainState(PlayerBrain brain, MovementController movement, FightController fight)
        {
            Brain = brain;
            Movement = movement;
            Fight = fight;
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