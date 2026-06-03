
using Core.Input;
using Core.UI;
using ExternalAssets.QuickOutline.Scripts;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BuggableTerminal: BuggableBase
    {
        [Inject] private Window _window;
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
            var puzzle = _puzzleView as TerminalPuzzleView; 
            puzzle.ShowPuzzle(this);
        
            // Отключаем управление игроком (как в BuggablePhysicsBox)
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