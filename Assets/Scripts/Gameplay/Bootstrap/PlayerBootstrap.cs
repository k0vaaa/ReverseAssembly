using Core.Events;
using Core.Extensions;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Anims;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.Core;
using Gameplay.Events;
using Gameplay.UI;
using Gameplay.UI.Views.Gameplay;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class PlayerBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [HideInInspector] public GameObject Player;

        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private Container _container;
        [Inject] private SyncEnergyManager _energyManager;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Camera _camera;
        [Inject] private GameplayWindow _gameplayWindow;

        private CooldownView _cooldownView;
        private StabilityBarView _stabilityBarView;
        private EnergyBarView _energyBarView;
        private TerminalView _terminalView;

        private StabilitySystem _playerStabilitySystem;
        private Container _playerContainer;
        private AbilitiesController _abilitiesController;
        private MovementController _movementController;
        private FightController _fightController;
        private PlayerBrain _brain;
        private WristTerminalController _wristTerminalController;
        private IPlayerAnimator _animator;

        public void Boot()
        {
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            Player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);

            GetComponents();

            print(_gameplayWindow.TryAddView(_terminalView) ? "okay" : "proebali");


            SetupContainer();

            GameObjectInjector.InjectRecursive(Player, _playerContainer);

            _brain.Init();
            _abilitiesController.Init();
            _movementController.Init();
            _fightController.Init();
            _wristTerminalController.Init();

            _cooldownView = _gameplayWindow.GetView<CooldownView>();
            _stabilityBarView = _gameplayWindow.GetView<StabilityBarView>();
            _energyBarView = _gameplayWindow.GetView<EnergyBarView>();

            EventBus.Subscribe<SyncEnergyChangedEvent>(e => _energyBarView.ChangeValue(e.EnergyPercent))
                .AddTo(gameObject);

            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeValue);

            // TODO настроить сейвы
            _playerStabilitySystem.Init(1);
            _playerStabilitySystem.SetStability(100);


            SetupCooldownListeners();
            
            // TODO настроить сейвы
            var currentSave = _playerDataInteractor.CurrentSave;
            if (currentSave != null && currentSave.Position != default)
            {
                Invoke(nameof(SetPos), 0.05f);
            }

            Player.SetActive(true);

            EventBus.Raise(new PlayerSpawnEvent
            {
                PlayerTransform = Player.transform,
                Camera = _camera
            });
        }

        private void GetComponents()
        {
            _ = new HudSwitcher(_gameplayWindow.GetView<PlayerHUDView>());
            _playerStabilitySystem = Player.GetComponent<StabilitySystem>();
            _abilitiesController = Player.GetComponent<AbilitiesController>();
            _movementController = Player.GetComponent<MovementController>();
            _fightController = Player.GetComponent<FightController>();
            _brain = Player.GetComponent<PlayerBrain>();
            _camera = Player.GetComponentInChildren<Camera>();
            _wristTerminalController = Player.GetComponent<WristTerminalController>();
            _animator = Player.GetComponent<IPlayerAnimator>();
            _terminalView = Player.GetComponentInChildren<TerminalView>(true);
        }

        private void SetupContainer()
        {
            _playerContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(_movementController);
                builder.RegisterValue(_fightController);
                builder.RegisterValue(_abilitiesController);
                builder.RegisterValue(_camera);
                builder.RegisterValue(_brain);
                builder.RegisterValue(_wristTerminalController);
                builder.RegisterValue(_animator, new []{typeof(MockPlayerAnimator)});
            });
        }

        private void SetupCooldownListeners()
        {
            _cooldownView.SetSlot2(_terminalView.SwitchBranchSkillView);
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

        private void SetPos()
        {
            if (Player != null && _playerDataInteractor != null)
            {
                Player.transform.position = _playerDataInteractor.CurrentSave.Position;
            }
        }

        private void OnDestroy()
        {
            _playerContainer?.Dispose();
            RemoveCooldownListeners();
        }
    }
}