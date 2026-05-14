using UnityEngine;
using Core.DI;
using Core.UI;
using Core.Input;
using Gameplay.UI;

namespace Gameplay.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuggablePhysicsBox : MonoBehaviour, IBuggable, IInjectable
    {
        public bool IsBugged { get; private set; } = true;

        [Inject] private ViewManager _viewManager;
        [Inject] private InputManager _inputManager;

        private Rigidbody _rb;
        private Outline _outline; // Опционально: компонент обводки

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();

            if (_outline) _outline.enabled = false;

            if (IsBugged)
            {
                _rb.mass = 9999f; // Игрок не сможет его сдвинуть
                _rb.isKinematic = true; // Для надежности
            }
        }

        public void OnScanned(bool isScanning)
        {
            if (!IsBugged) return;

            // Включаем красную обводку
            if (_outline != null)
            {
                _outline.enabled = isScanning;
                _outline.OutlineColor = Color.red;
            }

            var bugView = _viewManager.GetView<BugView>();
            bugView.Place(transform);
            bugView.SetVisible(isScanning);
            // В консоли можно вывести:[Property Error: Mass = 9999]
        }

        public void OnInteract()
        {
            if (!IsBugged) return;

            Debug.Log("Открытие мини-игры Синхронизации Физики...");

            if (_viewManager == null)
            {
                Debug.LogError("ViewManager не внедрен (NULL)! Проверьте DI контейнер.");
                return;
            }

            var puzzleView = _viewManager.GetView<PhysicsPuzzleView>();
            if (puzzleView == null)
            {
                Debug.LogError("PhysicsPuzzleView не найден! Вы добавили его в список ViewManager?");
                return;
            }

            puzzleView.ShowPuzzle(this);

            if (_inputManager == null)
            {
                Debug.LogError("InputManager не внедрен (NULL)!");
                return;
            }

            _inputManager.DisablePlayerInput();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void FixBug()
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
        }
    }
}