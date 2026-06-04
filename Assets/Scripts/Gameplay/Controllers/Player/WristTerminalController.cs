using Core.Bootstrap;
using Gameplay.Core;
using Gameplay.UI.Views.Gameplay.Terminal;
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
        [Inject] private TerminalWindow _terminalWindow;
        private TerminalView _terminalView;


        public bool IsTerminalOpen { get; private set; }

        public void Init()
        {
            _terminalView = _terminalWindow.GetView<TerminalView>();
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
                
                _terminalWindow.Show();
            }
            else
            {
                _terminalWindow.Hide();
            }
        }
    }
}