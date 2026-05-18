using Core.Events;
using Core.Input;
using Core.UI;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills.Definitions;
using Gameplay.Events;
using Gameplay.Interactables;
using Gameplay.StateMachines.PlayerStates.FightStates;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Abilities
{
    public class ScannerSkill : Skill
    {
        private readonly ScannerAbilityDefinition _def;
        private readonly SkillContext _ctx;
        [Inject] private ViewManager _viewManager;


        private IBuggable _currentTarget;
        private ScannerView _scannerView;
        [Inject] private Camera _cam;
        private Transform _cameraTransform;

        public bool IsScannerActive { get; private set; }

        public ScannerSkill(ScannerAbilityDefinition abilityDefinition, SkillContext ctx) : base(abilityDefinition)
        {
            _def = abilityDefinition;
            _ctx = ctx;
        }

        public override void Init()
        {
            _cameraTransform = _cam.transform;
            _scannerView = _viewManager.GetView<ScannerView>();
        }

        protected override void OnTick()
        {
            if (!IsScannerActive) return;

            // Рейкаст из центра экрана
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit,
                    _def.ScanDistance, _def.InteractableLayer))
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

        protected override bool CastAction()
        {
            if (!IsScannerActive)
            {
                if (_ctx.FightController.TryRequestState<ScannerState>())
                {
                    ToggleScanner();
                    return true;
                }

                return false;
            }
            else
            {
                if (_ctx.FightController.TryRequestState<IdleAttackState>())
                {
                    ToggleScanner();
                    return true;
                }

                return false;
            }
            
        }

        public void ToggleScanner()
        {
            IsScannerActive = !IsScannerActive;
            if (IsScannerActive)
            {
                _ctx.VFXController.PlayScanner();
                _scannerView?.FillIn();
            }
            else
            {
                _ctx.VFXController.PlayScanner(false);
                _scannerView?.FillOut();
            }

            //_scannerView?.SetVisible(IsScannerActive);

            if (!IsScannerActive && _currentTarget != null)
            {
                _currentTarget.OnScanned(false);
                _currentTarget = null;
            }
        }


        public void TryInteract()
        {
            if (IsScannerActive && _currentTarget != null)
            {
                _currentTarget.OnInteract(); // Запускаем мини-игру!
            }
        }
    }
}