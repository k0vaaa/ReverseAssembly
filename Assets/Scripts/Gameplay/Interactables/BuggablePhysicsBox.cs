using UnityEngine;

using Core.UI;
using Core.Input;
using ExternalAssets.QuickOutline.Scripts;
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;

namespace Gameplay.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuggablePhysicsBox : BuggableBase
    {
        
        [Inject] private InputManager _inputManager;
        private Rigidbody _rb;
        private BugView _bugView;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();

            if (_outline) _outline.enabled = false;
            _bugView = _windowManager.GetWindow<HUDWindow>().GetView<BugView>();
            
            if (IsBugged)
            {
                _rb.mass = 9999f; // Игрок не сможет его сдвинуть
                _rb.isKinematic = true; // Для надежности
            }
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

            _bugView.Place(transform);
            _bugView.SetVisible(isScanning);
            // В консоли можно вывести:[Property Error: Mass = 9999]
        }

        public override void OnInteract()
        {
            if (!IsBugged) return;

            Debug.Log("Открытие мини-игры Синхронизации Физики...");

            if (_windowManager == null)
            {
                Debug.LogError("ViewManager не внедрен (NULL)! Проверьте DI контейнер.");
                return;
            }

            
            if (_puzzleView != null)
            {
                _puzzleView.ShowPuzzle(this);
            }
            else
            {
                var puzzleView = _windowManager.GetWindow<TerminalWindow>().GetView<SliderPuzzleView>();
                if (puzzleView == null)
                {
                    Debug.LogError("PhysicsPuzzleView не найден! Вы добавили его в список ViewManager?");
                    return;
                }

                puzzleView.ShowPuzzle(this);
            }
            

            if (_inputManager == null)
            {
                Debug.LogError("InputManager не внедрен (NULL)!");
                return;
            }

            _inputManager.DisablePlayerInput();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void FixBug()
        {
            IsBugged = false;

            _rb.isKinematic = false;
            _rb.mass = 10f; // Нормальная масса

            if (_outline)
            {
                _outline.OutlineColor = Color.green; // Показываем, что починено
                Invoke(nameof(DisableOutline), 1f); // Выключаем через секунду
            }

            Debug.Log("Ящик починен! Теперь его можно двигать.");
        }

        private void DisableOutline()
        {
            if (_outline) _outline.enabled = false;
            _bugView.Hide();
        }
    }
}