using System;
using Core.Bootstrap;
using Gameplay.Core;
using Gameplay.StateMachines.PlayerStates.PlayerBrainStates;
using Gameplay.UI;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;
using InputManager = Core.Input.InputManager;

namespace Gameplay.Controllers.Player
{
    public class WristTerminalController : MonoBehaviour, IInitializable
    {
        [Inject] private InputManager _inputManager;
        [Inject] private SyncEnergyManager _syncEnergyManager;
        [Inject] private PlayerBrain _brain;
        [Inject] private GameplayWindow _gameplayWindow;
        private TerminalView _terminalView;


        public bool IsTerminalOpen { get; private set; }

        public void Init()
        {
            _terminalView = _gameplayWindow.GetView<TerminalView>();
        }


        public void ToggleTerminal()
        {
            IsTerminalOpen = !IsTerminalOpen;

            SetTerminal(IsTerminalOpen);
        }



        public void SetTerminal(bool active)
        {
            if (active)
            {
                _terminalView.UpdateInfo(BranchManager.CurrentBranch);
                _terminalView.Show();
            }
            else
            {
                _terminalView.Hide();
            }
        }
    }
}