using System;
using UnityEngine;
using Core.DI;
using Core.UI;
using Core.Input;
using Gameplay.Core;
using Gameplay.Events;
using Gameplay.UI;
using Unity.VisualScripting;

namespace Gameplay.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuggablePhysicsBox : BuggableBase, IInjectable
    {
        [Inject] private BranchManager _branchManager;
        [Inject] private ViewManager _viewManager;
        [Inject] private InputManager _inputManager;
        [SerializeField] private GameObject _objectToActivate;
        [SerializeField] private bool _enablePhysicsOnFix = true; 
        private Rigidbody _rb;
        private Outline _outline; // Опционально: компонент обводки
        private BugView _bugView;
        

        private void Awake()
        {
            if (_objectToActivate != null) _objectToActivate.SetActive(false);
            _rb = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();

            if (_outline) _outline.enabled = false;
            _bugView = _viewManager.GetView<BugView>();
            
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

            if (_viewManager == null)
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
                var puzzleView = _viewManager.GetView<PhysicsPuzzleView>();
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

            if (_enablePhysicsOnFix) // Проверяем флаг
            {
                _rb.isKinematic = false;
                _rb.mass = 10f;
            }
            if (_objectToActivate != null)
            {
                _objectToActivate.SetActive(true);
            }

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