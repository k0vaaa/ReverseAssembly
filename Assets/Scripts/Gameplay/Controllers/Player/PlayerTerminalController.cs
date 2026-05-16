using Core.Bootstrap;

using Core.Input;
using Core.UI;
using Gameplay.Core;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class PlayerTerminalController : MonoBehaviour, IInitializable
    {
        [Inject] private InputManager _inputManager;
        [Inject] private ViewManager _viewManager;
        [Inject] private BranchManager _branchManager;

        private TerminalView _terminalView;
        private bool _isTerminalOpen;

        public void Init()
        {
            _inputManager.OnBranchTogglePressed += ToggleTerminal;
            //_inputManager.OnInteractPressed += TryJump;
        }

        private void ToggleTerminal()
        {
            if (_terminalView == null)
            {
                _terminalView = _viewManager.GetView<TerminalView>();
                if (_terminalView == null)
                {
                    Debug.LogError("TerminalView не найден в ViewManager!");
                    return;
                }
            }

            _isTerminalOpen = !_isTerminalOpen;

            if (_isTerminalOpen)
            {
                _terminalView.UpdateInfo(_branchManager.CurrentBranch);
                _terminalView.Show();
            }
            else
            {
                _terminalView.Hide();
            }
        }

        public bool TryJump()
        {
            if (!_isTerminalOpen) return false;

            // Выполняем прыжок
            _branchManager.ToggleBranch();
            
            // Закрываем терминал после прыжка
            ToggleTerminal();
            return true;
        }

        private void OnDestroy()
        {
            if (_inputManager != null)
            {
                _inputManager.OnBranchTogglePressed -= ToggleTerminal;
                //_inputManager.OnInteractPressed -= TryJump;
            }
        }
    }
}
