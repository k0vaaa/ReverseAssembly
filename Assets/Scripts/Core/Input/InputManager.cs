using System;
using Core.Bootstrap;
using UnityEngine;
using static InputSystemActions;
using static UnityEngine.InputSystem.InputAction;

namespace Core.Input
{
    public class InputManager : IInitializable
    {
        private InputSystemActions _inputSystemActions;
        private PlayerActions _playerActions;
        private UIActions _uiActions;

        public event Action OnEscapePressed;
        public event Action OnJumpPressed;
        public event Action OnRightClick;

        public Vector2 MoveInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool MeleeInput { get; private set; }
        public bool RMBInput { get; private set; }

        //public bool SpellInput { get; private set; }
        //public bool IsSheathed { get; private set; }

        public void Init()
        {
            _inputSystemActions = new InputSystemActions();
            _playerActions = _inputSystemActions.Player;
            _uiActions = _inputSystemActions.UI;
            _playerActions.Enable();
            _uiActions.Enable();
            Subscribe();
            SubscribePlayerActions();
        }

        private void Subscribe()
        {
            _uiActions.Cancel.performed += ctx => OnEscapePressed?.Invoke();
            _playerActions.Jump.performed += OnJumpPress;
            _playerActions.RightClick.performed += OnRightClickPress;
        }

        private void SubscribePlayerActions()
        {
            _playerActions.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _playerActions.Move.canceled += ctx => MoveInput = Vector2.zero;
            _playerActions.Sprint.performed += ctx => SprintInput = true;
            _playerActions.Sprint.canceled += ctx => SprintInput = false;
            _playerActions.Attack.performed += ctx => MeleeInput = true;
            _playerActions.Attack.canceled += ctx => MeleeInput = false;
            //_playerActions.Spell.performed += ctx => SpellInput = true;
            //_playerActions.Spell.canceled += ctx => SpellInput = false;
            _playerActions.RightClick.performed += ctx => RMBInput = true;
            _playerActions.RightClick.canceled += ctx => RMBInput = false;
            //_playerActions.Sheath.performed += ctx => IsSheathed = !IsSheathed;
        }

        private void OnJumpPress(CallbackContext ctx) => OnJumpPressed?.Invoke();
        private void OnRightClickPress(CallbackContext ctx) => OnRightClick?.Invoke();

        
        public void Dispose()
        {
            _inputSystemActions.Dispose();
        }
    }
}