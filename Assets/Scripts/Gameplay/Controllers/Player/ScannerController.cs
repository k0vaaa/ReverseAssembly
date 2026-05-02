using System;
using Core.Bootstrap;
using Core.DI;
using Gameplay.Interactables;
using UnityEngine;
using InputManager = Core.Input.InputManager;

namespace Gameplay.Controllers.Player
{
    public class ScannerController : MonoBehaviour, IInjectable, IInitializable, IDisposable
    {
        [Inject] private InputManager _inputManager;
        // [Inject] private ViewManager _viewManager; // Понадобится позже для UI сканера

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _scanDistance = 10f;
        [SerializeField] private LayerMask _interactableLayer;

        public bool IsScannerActive { get; private set; }
        private IBuggable _currentTarget;

        public void Init()
        {
            _inputManager.OnScannerPressed += ToggleScanner;
            _inputManager.OnInteractPressed += TryInteract;
        }

        private void ToggleScanner()
        {
            IsScannerActive = !IsScannerActive;
            
            // TODO: Сообщить ViewManager включить синюю рамку на экране
            // TODO: Включить PostProcessing (изменение цвета мира)

            if (!IsScannerActive && _currentTarget != null)
            {
                _currentTarget.OnScanned(false);
                _currentTarget = null;
            }
        }

        private void Update()
        {
            if (!IsScannerActive) return;

            // Рейкаст из центра экрана
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _scanDistance, _interactableLayer))
            {
                var buggable = hit.collider.GetComponent<IBuggable>();
                
                if (buggable != null && buggable.IsBugged)
                {
                    if (_currentTarget != buggable)
                    {
                        _currentTarget?.OnScanned(false);
                        _currentTarget = buggable;
                        _currentTarget.OnScanned(true); // Подсвечиваем красным
                    }
                }
            }
            else
            {
                if (_currentTarget != null)
                {
                    _currentTarget.OnScanned(false);
                    _currentTarget = null;
                }
            }
        }

        private void TryInteract()
        {
            if (IsScannerActive && _currentTarget != null)
            {
                _currentTarget.OnInteract(); // Запускаем мини-игру!
            }
        }

        public void Dispose()
        {
            _inputManager.OnScannerPressed -= ToggleScanner;
            _inputManager.OnInteractPressed -= TryInteract;
        }
    }
}