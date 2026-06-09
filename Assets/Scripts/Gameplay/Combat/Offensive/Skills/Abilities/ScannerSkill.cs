using Gameplay.Abilities;
using Gameplay.Core;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills.Definitions;
using Gameplay.Controllers.Player;
using Gameplay.Interactables;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Skills.Abilities
{
    public class ScannerSkill : Skill
    {
        private readonly ScannerAbilityDefinition _def;
        private readonly SkillContext _ctx;

        [Inject] private HUDWindow _hudWindow;
        [Inject] private VFXController _vfx;


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
            _scannerView = _hudWindow.GetView<ScannerView>();
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
                if (buggable != null && buggable.IsBugged &&
                    buggable.IsInteractableInCurrentBranch(BranchManager.CurrentBranch))
                {
                    if (_currentTarget != buggable)
                    {
                        _currentTarget?.Scan(false);
                        _currentTarget = buggable;
                        _currentTarget.Scan(true);
                        
                        _vfx.SetProjection(_currentTarget.GetMesh());
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
            ToggleScanner();
            return true;
        }

        private void ToggleScanner()
        {
            IsScannerActive = !IsScannerActive;
            SetScanner(IsScannerActive);
        }

        public void SetScanner(bool active)
        {
            IsScannerActive = active;
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
            
            if (!IsScannerActive && _currentTarget != null)
            {
                _currentTarget.Scan(false);
                _currentTarget = null;
            }
        }


        public void TryInteract()
        {
            if (IsScannerActive && _currentTarget != null)
            {
                if (_currentTarget.IsInteractableInCurrentBranch(BranchManager.CurrentBranch))
                {
                    _currentTarget.OnInteract();
                }
                else
                {
                    Debug.Log("Объект недоступен в этой реальности!");
                }
            }
        }


        private void ResetTarget()
        {
            if (_currentTarget != null)
            {
                _currentTarget.Scan(false);
                _currentTarget = null;
                _vfx.SetProjection(null);
            }
        }
    }
}