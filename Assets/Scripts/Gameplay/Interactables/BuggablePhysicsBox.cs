using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuggablePhysicsBox : MonoBehaviour, IBuggable
    {
        public bool IsBugged { get; private set; } = true;

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
                // TODO Добавить обоводку
                //_outline.OutlineColor = Color.red;
            }

            // В консоли можно вывести:[Property Error: Mass = 9999]
        }

        public void OnInteract()
        {
            if (!IsBugged) return;

            Debug.Log("Открытие мини-игры Синхронизации Физики...");

            // В СЛЕДУЮЩЕМ ЭТАПЕ: 
            // 1. Отключаем инпут игрока
            // 2. _viewManager.GetView<PhysicsPuzzleView>().Show(this);

            // Пока мини-игры нет, чиним сразу для теста:
            FixBug();
        }

        public void FixBug()
        {
            IsBugged = false;

            _rb.isKinematic = false;
            _rb.mass = 10f; // Нормальная масса

            if (_outline)
            {
                //_outline.OutlineColor = Color.green; // Показываем, что починено
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