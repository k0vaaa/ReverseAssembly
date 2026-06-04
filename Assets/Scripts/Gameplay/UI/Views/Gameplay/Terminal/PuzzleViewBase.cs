using Core.Input;
using Core.UI;
using Gameplay.Interactables;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.UI.Views.Gameplay.Terminal
{
    public abstract class PuzzleViewBase : View
    {
        [Inject] protected InputManager _inputManager;
        [Inject] private WindowManager _windowManager;
        protected IBuggable _currentTarget;

        public virtual void ShowPuzzle(IBuggable target)
        {
            _currentTarget = target;
            Show();
            _windowManager.GetWindow<TerminalWindow>().GetView<TerminalScannerView>().Hide();
        }

        public virtual void ClosePuzzle()
        {
            Hide();
            _windowManager.GetWindow<TerminalWindow>().GetView<TerminalScannerView>().Show();
            _currentTarget = null;
            
            _inputManager.EnablePlayerInput();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected void OnWin()
        {
            _currentTarget?.FixBug();
            ClosePuzzle();
        }
    }
}
