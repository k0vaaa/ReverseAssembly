using System;
using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Gameplay.Interactables;
using UnityEngine;
using InputManager = Core.Input.InputManager;

using Gameplay.UI;
using Core.UI;
using Gameplay.Core;
using Gameplay.Events;

namespace Gameplay.Controllers.Player
{
    public class ScannerController : MonoBehaviour, IInjectable, IInitializable, IDisposable
    {
        [Inject] private BranchManager _branchManager;

        [Inject] private InputManager _inputManager;
        [Inject] private ViewManager _viewManager;

        [SerializeField] private float _scanDistance = 10f;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private ParticleSystem _particleSystem;

        public bool IsScannerActive { get; private set; }
        private IBuggable _currentTarget;
        private ScannerView _scannerView;
        private Transform _cameraTransform;


        public void Init()
        {
            EventBus.Subscribe<PlayerSpawnEvent>(HandlePlayerSpawn).AddTo(gameObject);
            _scannerView = _viewManager.GetView<ScannerView>();
            //_inputManager.OnScannerPressed += ToggleScanner;
            _inputManager.OnInteractPressed += TryInteract;
        }

        private void HandlePlayerSpawn(PlayerSpawnEvent e)
        {
            _cameraTransform = e.Camera.transform;
        }

        public void ToggleScanner()
        {
            IsScannerActive = !IsScannerActive;
            if (IsScannerActive)
            {
                _particleSystem.Play();
                _scannerView?.FillIn();
            }
            else
            {
                _particleSystem.Stop();
                _scannerView?.FillOut();
            }
            
            //_scannerView?.SetVisible(IsScannerActive);

            if (!IsScannerActive && _currentTarget != null)
            {
                _currentTarget.OnScanned(false);
                _currentTarget = null;
            }
        }

        private void Update()
        {
            if (!IsScannerActive) return;

            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _scanDistance, _interactableLayer))
            {
                var buggable = hit.collider.GetComponent<IBuggable>();
        
                // ПРОВЕРКА: если объект багнут И находится в нужной ветке
                if (buggable != null && buggable.IsBugged && buggable.IsInteractableInCurrentBranch(_branchManager.CurrentBranch))
                {
                    if (_currentTarget != buggable)
                    {
                        _currentTarget?.OnScanned(false);
                        _currentTarget = buggable;
                        _currentTarget.OnScanned(true); 
                    }
                }
                else
                {
                    ResetTarget();
                }
            }
            else
            {
                ResetTarget();
            }
        }


        private void TryInteract()
        {
            if (IsScannerActive && _currentTarget != null)
                if (_currentTarget.IsInteractableInCurrentBranch(_branchManager.CurrentBranch))
                {
                    _currentTarget.OnInteract();
                }
                else
                {
                    Debug.Log("Объект недоступен в этой реальности!");
                }
        }

        public void Dispose()
        {
            //_inputManager.OnScannerPressed -= ToggleScanner;
            _inputManager.OnInteractPressed -= TryInteract;
        }
        private void ResetTarget()
        {
            if (_currentTarget != null)
            {
                _currentTarget.OnScanned(false);
                _currentTarget = null;
            }
        }
    }
}