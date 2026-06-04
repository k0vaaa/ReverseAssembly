
using Core.Input;
using ExternalAssets.QuickOutline.Scripts;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BuggableTerminal: BuggableBase
    {
        [Inject] private InputManager _inputManager;
        [SerializeField] private GameObject bridgeObj;
        
        private void Awake()
        {
            _outline = GetComponent<Outline>();

            if (_outline) _outline.enabled = false;
            // Изначально мост скрыт
            if (bridgeObj != null) bridgeObj.SetActive(false);
            
        }

        public override void OnScanned(bool isScanning)
        {
            if (!IsBugged) return;

            // Включаем красную обводку
            if (_outline != null)
            {
                _outline.enabled = isScanning;
                _outline.OutlineColor = Color.red;
            }
        }

        public override void OnInteract()
        {
            // Ищем наш новый PuzzleView
            if (_puzzleView != null)
            {
                _puzzleView.ShowPuzzle(this);
            }
            else
            {
                _puzzleView = _windowManager.GetWindow<TerminalWindow>().GetView<TerminalPuzzleView>();
            }
            
            
        
            
            _inputManager.DisablePlayerInput();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void FixBug()
        {
            Debug.Log("Терминал починен!");
            IsBugged = false;
            if (bridgeObj != null) bridgeObj.SetActive(true);
            if (_outline)
            {
                _outline.OutlineColor = Color.green; // Показываем, что починено
                Invoke(nameof(DisableOutline), 1f); // Выключаем через секунду
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void DisableOutline()
        {
            if (_outline) _outline.enabled = false;
        }
    }
}