using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class DefaultState : BrainState
    {
        public DefaultState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = true;
        }
    }
}