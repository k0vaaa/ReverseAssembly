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
        private bool _isInitialized;

        public event Action OnEscapePressed;
        public event Action OnJumpPressed;
        public event Action OnRightClick;
        public event Action OnLeftClick;
        
        public event Action OnInteractPressed;
        public event Action OnScannerPressed;
        public event Action OnTerminalPressed;

        public event Action OnSlotOnePressed;
        public event Action OnSlotTwoPressed;

        public Vector2 MoveInput { get; private set; }
        public Vector2 MouseInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool MeleeInput { get; private set; }
        public bool RMBInput { get; private set; }

        //public bool SpellInput { get; private set; }
        //public bool IsSheathed { get; private set; }

        public void Init()
        {
            if (_isInitialized) return;
            _inputSystemActions = new InputSystemActions();
            _playerActions = _inputSystemActions.Player;
            _uiActions = _inputSystemActions.UI;
            _inputSystemActions.Enable();
            _playerActions.Enable();
            _uiActions.Enable();
            Subscribe();
            SubscribePlayerActions();
            _isInitialized = true;
        }

        private void Subscribe()
        {
            _uiActions.Cancel.performed += _ => OnEscapePressed?.Invoke();
            
            _playerActions.Jump.performed += OnJumpPress;
            _playerActions.RightClick.performed += OnRightClickPress;
            _playerActions.Interact.performed += OnInteractPress;
            _playerActions.Scanner.performed += OnScannerPress;
            _playerActions.Terminal.performed += OnTerminalPress;
            _playerActions.SlotOne.performed += OnSlotOnePress;
            _playerActions.SlotTwo.performed += OnSlotTwoPress;
            _playerActions.LeftClick.performed += OnLeftClickPress;
        }

        private void SubscribePlayerActions()
        {
            _playerActions.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _playerActions.Move.canceled += ctx => MoveInput = Vector2.zero;
            _playerActions.Look.performed += ctx => MouseInput = ctx.ReadValue<Vector2>();
            _playerActions.Look.canceled += ctx => MouseInput = Vector2.zero;
            _playerActions.Sprint.performed += ctx => SprintInput = true;
            _playerActions.Sprint.canceled += ctx => SprintInput = false;
            _playerActions.LeftClick.performed += ctx => MeleeInput = true;
            _playerActions.LeftClick.canceled += ctx => MeleeInput = false;
            //_playerActions.Spell.performed += ctx => SpellInput = true;
            //_playerActions.Spell.canceled += ctx => SpellInput = false;
            _playerActions.RightClick.performed += ctx => RMBInput = true;
            _playerActions.RightClick.canceled += ctx => RMBInput = false;
            //_playerActions.Sheath.performed += ctx => IsSheathed = !IsSheathed;
        }

        private void OnJumpPress(CallbackContext ctx) => OnJumpPressed?.Invoke();
        private void OnRightClickPress(CallbackContext ctx) => OnRightClick?.Invoke();
        private void OnLeftClickPress(CallbackContext ctx) => OnLeftClick?.Invoke();
        private void OnInteractPress(CallbackContext ctx) => OnInteractPressed?.Invoke();
        private void OnScannerPress(CallbackContext ctx) => OnScannerPressed?.Invoke();
        private void OnTerminalPress(CallbackContext ctx) => OnTerminalPressed?.Invoke();
        private void OnSlotOnePress(CallbackContext ctx) => OnSlotOnePressed?.Invoke();
        private void OnSlotTwoPress(CallbackContext ctx) => OnSlotTwoPressed?.Invoke();



        public void DisablePlayerInput()
        {
            _playerActions.Disable();
            MoveInput = Vector2.zero;
            SprintInput = false;
        }

        public void EnablePlayerInput()
        {
            _playerActions.Enable();
        }

        public void DisableEsc()
        {
            _inputSystemActions.UI.Cancel.Disable();
        }
        
    }
}