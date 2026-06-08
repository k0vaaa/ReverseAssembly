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
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class PlayerBootstrap : BootstrapComponent
    {
        [HideInInspector] public GameObject Player;

        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private SaveManager _saveManager;
        [Inject] private Container _container;
        [Inject] private SyncEnergyManager _energyManager;
        [Inject] private WindowManager _windowManager;
        [Inject] private HUDWindow _hudWindow;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Camera _camera;


        private TerminalWindow _terminalWindow;

        private Container _playerContainer;

        public Container PlayerContainer => _playerContainer;

        private StabilitySystem _playerStabilitySystem;
        private AbilitiesController _abilitiesController;
        private MovementController _movementController;
        private FightController _fightController;
        private PlayerBrain _brain;
        private WristTerminalController _wristTerminalController;
        private IPlayerAnimator _animator;

        protected override void OnBoot()
        {
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            Player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);

            GetComponents();

            SetupContainer();

            GameObjectInjector.InjectRecursive(Player, _playerContainer);

            _brain.Init();
            _abilitiesController.Init();
            _movementController.Init();
            _fightController.Init();
            _wristTerminalController.Init();



            _playerStabilitySystem.Init(1);
            _playerStabilitySystem.SetStability(100);
            
            _saveManager.SetPlayerSystems(_playerStabilitySystem, _movementController);
            _saveManager.LoadPlayerAndPuzzles(_playerStabilitySystem, _movementController);

            Player.SetActive(true);

            EventBus.Raise(new PlayerSpawnEvent
            {
                PlayerTransform = Player.transform,
                Camera = _camera
            });
        }

        private void GetComponents()
        {
            _ = new HudSwitcher(_hudWindow.GetView<PlayerHUDView>());
            _playerStabilitySystem = Player.GetComponent<StabilitySystem>();
            _abilitiesController = Player.GetComponent<AbilitiesController>();
            _movementController = Player.GetComponent<MovementController>();
            _fightController = Player.GetComponent<FightController>();
            _brain = Player.GetComponent<PlayerBrain>();
            _camera = Player.GetComponentInChildren<Camera>();
            _wristTerminalController = Player.GetComponent<WristTerminalController>();
            _animator = Player.GetComponent<IPlayerAnimator>();
            _terminalWindow = Player.GetComponentInChildren<TerminalWindow>(true);
            _windowManager.TryAdd(_terminalWindow);
            print("TerminalWindow Added to WindowManager");

        }

        private void SetupContainer()
        {
            var vfx = Player.GetComponent<VFXController>();
            _playerContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(_movementController);
                builder.RegisterValue(_fightController);
                builder.RegisterValue(_abilitiesController);
                builder.RegisterValue(_playerStabilitySystem);
                builder.RegisterValue(_camera);
                builder.RegisterValue(_brain);
                builder.RegisterValue(_wristTerminalController);
                builder.RegisterValue(_animator, new []{typeof(MockPlayerAnimator)});
                builder.RegisterValue(_terminalWindow);
                builder.RegisterValue(vfx);
            });
        }

        private void OnDestroy()
        {
            _playerContainer?.Dispose();
        }
    }
}