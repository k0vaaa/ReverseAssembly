using Core.UI;
using Core.Input;

using Gameplay.Interactables;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.UI
{
    public abstract class PuzzleViewBase : View
    {
        [Inject] protected InputManager _inputManager;
        protected IBuggable _currentTarget;

        public virtual void ShowPuzzle(IBuggable target)
        {
            _currentTarget = target;
            Show();
        }

        public virtual void ClosePuzzle()
        {
            Hide();
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
