using Core.UI;
using Gameplay.Abilities;
using Gameplay.Core;

using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills.Definitions;
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

        [Inject] private Window _window;


        private IBuggable _currentTarget;
        private ScannerView _scannerView;
        [Inject] private Camera _cam;
        private Transform _cameraTransform;
        private ScannerTrigger _scannerTrigger;

        public bool IsScannerActive { get; private set; }

        public ScannerSkill(ScannerAbilityDefinition abilityDefinition, SkillContext ctx) : base(abilityDefinition)
        {
            _def = abilityDefinition;
            _ctx = ctx;
        }

        public override void Init()
        {
            _cameraTransform = _cam.transform;
            _scannerView = _window.GetView<ScannerView>();
            _scannerTrigger = _ctx.Caster.GetComponentInChildren<ScannerTrigger>();
            _scannerTrigger.OnTriggerEntered += HandleTrigger;
        }

        private void HandleTrigger(Collider other)
        {
            if (other.TryGetComponent(out IBuggable buggable))
            {
                buggable.Visualize();
            }
        }

        protected override void OnTick()
        {
            if (!IsScannerActive) return;


            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit,
                    _def.ScanDistance, _def.InteractableLayer))
            {
                var buggable = hit.collider.GetComponent<IBuggable>();
        
                // ПРОВЕРКА: если объект багнут И находится в нужной ветке
                if (buggable != null && buggable.IsBugged && buggable.IsInteractableInCurrentBranch(BranchManager.CurrentBranch))
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
                _scannerTrigger.Expand();
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
                if (_currentTarget.IsInteractableInCurrentBranch(BranchManager.CurrentBranch))
                {
                    _currentTarget.OnInteract();
                }
                else
                {
                    Debug.Log("Объект недоступен в этой реальности!");
                }
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