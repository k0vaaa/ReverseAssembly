using Core.Events;
using Core.Extensions;
using Core.UI;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.Events;
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    [RequireComponent(typeof(PlayerBootstrap))]
    public class InterfaceBootstrap :  BootstrapComponent
    {
        //Components
        private StabilitySystem _playerStabilitySystem;
        private AbilitiesController _abilitiesController;

        [Inject] private WindowManager _windowManager;
        [Inject] private HUDWindow _hudWindow;
        private TerminalWindow _terminalWindow;
        
        //Views
        private CooldownView _cooldownView;
        private StabilityBarView _stabilityBarView;
        private EnergyBarView _energyBarView;
        private PlayerBootstrap _playerBootstrap;

        protected override void OnBoot()
        {
            _playerBootstrap = GetComponent<PlayerBootstrap>();
            
            GetComponents();

            EventBus.Subscribe<SyncEnergyChangedEvent>(e => _energyBarView.ChangeValue(e.EnergyPercent))
                .AddTo(gameObject);

            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeValue);
            
            SetupCooldownListeners();
        }

        private void GetComponents()
        {
            _cooldownView = _hudWindow.GetView<CooldownView>();
            _stabilityBarView = _hudWindow.GetView<StabilityBarView>();
            _energyBarView = _hudWindow.GetView<EnergyBarView>();
            _playerStabilitySystem = _playerBootstrap.Player.GetComponent<StabilitySystem>();
            _abilitiesController = _playerBootstrap.Player.GetComponent<AbilitiesController>();
            _terminalWindow = _windowManager.GetWindow<TerminalWindow>();
        }

        private void OnDestroy()
        {
            RemoveCooldownListeners();
        }

        private void SetupCooldownListeners()
        {
            _cooldownView.SetSlot1(_terminalWindow.GetView<TerminalScannerView>().ScannerSkillView);
            _cooldownView.SetSlot2(_terminalWindow.GetView<TerminalView>().SwitchBranchSkillView);
            _cooldownView.ResetAll();
            _abilitiesController.TryGetSkill<ScannerSkill>().OnCooldownTick += _cooldownView.SetSlot1FillAmount;
            _abilitiesController.TryGetSkill<SwitchBranchSkill>().OnCooldownTick += _cooldownView.SetSlot2FillAmount;
            _abilitiesController.TryGetSkill<MeleeSkill>().OnCooldownTick += _cooldownView.SetSlot3FillAmount;
            _abilitiesController.TryGetSkill<ProjectileSkill>().OnCooldownTick += _cooldownView.SetSlot4FillAmount;
        }

        private void RemoveCooldownListeners()
        {
            _abilitiesController.TryGetSkill<ScannerSkill>().OnCooldownTick -= _cooldownView.SetSlot1FillAmount;
            _abilitiesController.TryGetSkill<SwitchBranchSkill>().OnCooldownTick -= _cooldownView.SetSlot2FillAmount;
            _abilitiesController.TryGetSkill<MeleeSkill>().OnCooldownTick -= _cooldownView.SetSlot3FillAmount;
            _abilitiesController.TryGetSkill<ProjectileSkill>().OnCooldownTick -= _cooldownView.SetSlot4FillAmount;
        }
    }
}