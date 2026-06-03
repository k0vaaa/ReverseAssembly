using Core.Input;
using Gameplay.Controllers.Player;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class EndGameState : BrainState
    {
        [Inject] private InputManager _input;
        public EndGameState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
        }

        public override void Enter()
        {
            Movement.enabled = false;
            Fight.enabled = false;
            _input.DisablePlayerInput();
            _input.DisableEsc();
        }
    }
}