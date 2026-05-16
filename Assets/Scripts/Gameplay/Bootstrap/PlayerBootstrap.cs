
using Core.Events;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using Gameplay.Events;
using Gameplay.UI.Views.Gameplay;
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
        
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Camera _camera;
        [Inject] private ViewManager _viewManager;
        
        private CooldownView _cooldownView;
        private StabilityBarView _stabilityBarView;

        private StabilitySystem _playerStabilitySystem;

        public void Boot()
        {
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            Player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
            GameObjectInjector.InjectRecursive(Player,_container);
            _camera = Player.GetComponentInChildren<Camera>();
            _playerStabilitySystem = Player.GetComponent<StabilitySystem>();
            var movementController = Player.GetComponent<MovementController>();
            var skillsController = Player.GetComponent<SkillsController>();
            var scannerController = Player.GetComponent<ScannerController>();
            var terminalController = Player.GetComponent<PlayerTerminalController>();

            _stabilityBarView = _viewManager.GetView<StabilityBarView>();
            _cooldownView = _viewManager.GetView<CooldownView>();
            
            scannerController.Init();
            terminalController.Init();
            
            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeHp);
            
            // TODO настроить сейвы
            _playerStabilitySystem.Init(1);
            _playerStabilitySystem.SetStability(100);


            
            skillsController.Init(_camera);
            SetupCooldownListeners(skillsController);
            // TODO настроить сейвы
            var currentSave = _playerDataInteractor.CurrentSave;
            if (currentSave != null && currentSave.Position != default)
            {
                // Запускаем корутину или просто меняем позицию через задержку.
                // Так как это не MonoBehaviour, используем сам Player для корутины или Invoke.
                var monoBehaviour = Player.GetComponent<MonoBehaviour>();
                monoBehaviour.Invoke(nameof(SetPos), 0.05f);
            }
            EventBus.Raise(new PlayerSpawnEvent()
            {
                PlayerTransform = Player.transform,
                Camera = _camera
            });
            
        }

        private void SetupCooldownListeners(SkillsController skillsController)
        {
            _cooldownView.SetSlot1Listener(() =>
                _cooldownView.SetSlot1FillAmount(skillsController.Skills[SkillType.BranchSwitch].GetReadyPercent()));
            _cooldownView.SetSlot2Listener(() =>
                _cooldownView.SetSlot2FillAmount(skillsController.Skills[SkillType.Scanner].GetReadyPercent()));
        }

        private void SetPos()
        {
            if (Player != null && _playerDataInteractor != null)
            {
                Player.transform.position = _playerDataInteractor.CurrentSave.Position;
            }
        }
    }
}