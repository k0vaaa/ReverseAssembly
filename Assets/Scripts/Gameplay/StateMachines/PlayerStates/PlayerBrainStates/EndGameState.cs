using Core.Input;
using Core.Pause;
using Gameplay.Controllers.Player;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Attributes;
using UnityEngine;
using Logger = Core.Utilities.Logger;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class EndGameState : BrainState
    {
        [Inject] private InputManager _input;
        [Inject] private HUDWindow _hudWindow;
        public EndGameState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
            
        }

        protected override void EnterAction()
        {
            Logger.LogTagged("EndGameState", "states");
            PauseManager.SetPause(true);
            _hudWindow.ShowOnly<EndGameView>();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Movement.enabled = false;
            Fight.enabled = false;
            _input.DisablePlayerInput();
            _input.DisableEsc();
            
            
        }
    }
}