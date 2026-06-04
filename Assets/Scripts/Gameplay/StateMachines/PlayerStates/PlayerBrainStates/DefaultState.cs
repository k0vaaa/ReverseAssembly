using Core.Input;
using Gameplay.Controllers.Player;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class DefaultState : BrainState
    {
        [Inject] private InputManager _input;
        public DefaultState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = true;
            _input.EnablePlayerInput();
        }
    }
}