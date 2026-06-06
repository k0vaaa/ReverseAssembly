using Core.Input;
using Core.Pause;
using Core.Scenes;
using Core.UI;
using Gameplay.Controllers.Player;
using Gameplay.UI.Views.Gameplay;
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class PauseState : BrainState
    {
        [Inject] private InputManager _input;
        [Inject] private WindowManager _windowManager;
        [Inject] private SceneLoader _sceneLoader;
        private PauseView _pauseView;

        public PauseState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
        }

        public override void Enter()
        {
            PauseManager.SetPause(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Movement.enabled = false;
            Fight.enabled = false;
            _input.DisablePlayerInput();
            _windowManager.Show<MenuWindow>();
            _windowManager.Hide<HUDWindow>();
            _pauseView = _windowManager.GetWindow<MenuWindow>().GetView<PauseView>();
            Sub();
            
        }

        public override void Exit()
        {
            Unsub();
            _windowManager.Show<HUDWindow>();
            _windowManager.Hide<MenuWindow>();
            Movement.enabled = true;
            Fight.enabled = true;
            _input.EnablePlayerInput();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseManager.SetPause(false);
        }

        private void Sub()
        {
            _pauseView.ExitClick += HandleExit;
            _pauseView.ResumeClick += HandleResume;
            _pauseView.MainMenuClick += HandleMenu;
        }
        private void Unsub()
        {
            _pauseView.ExitClick -= HandleExit;
            _pauseView.ResumeClick -= HandleResume;
            _pauseView.MainMenuClick -= HandleMenu;
        }


        private void HandleExit() => Application.Quit();
        private void HandleResume() => Brain.StateMachine.ForcePreviousState();
        private void HandleMenu() => _sceneLoader.LoadScene(SceneConstants.MainMenu);
    }
}