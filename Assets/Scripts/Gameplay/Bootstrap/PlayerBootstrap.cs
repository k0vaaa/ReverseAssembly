using Core.DI;
using Core.Events;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using Gameplay.Events;
using Gameplay.UI.Views.Gameplay;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class PlayerBootstrap
    {
        private PlayerDataInteractor _playerDataInteractor;

        private readonly DIContainer _diContainer;
        private readonly Transform _playerSpawnPoint;
        private readonly GameObject _playerPrefab;
        private  Camera _camera;
        private readonly CooldownView _cooldownView;
        private readonly StabilityBarView _stabilityBarView;

        private GameObject _player;
        private StabilitySystem _playerStabilitySystem;

        public PlayerBootstrap(
            PlayerDataInteractor playerDataInteractor,
            Transform playerSpawnPoint,
            GameObject playerPrefab,
            Camera camera,
            CooldownView cooldownView,
            StabilityBarView stabilityBarView,
            DIContainer diContainer)
        {
            _playerDataInteractor = playerDataInteractor;
            _playerSpawnPoint = playerSpawnPoint;
            _playerPrefab = playerPrefab;
            _cooldownView = cooldownView;
            _stabilityBarView = stabilityBarView;
            _diContainer = diContainer;
        }

        public GameObject SetupPlayer()
        {
            _player = Object.Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
            _camera = _player.GetComponentInChildren<Camera>();
            _playerStabilitySystem = _player.GetComponent<StabilitySystem>();
            var movementController = _player.GetComponent<MovementController>();
            var fightController = _player.GetComponent<FightController>();
            var skillsController = _player.GetComponent<SkillsController>();
            var scannerController = _player.GetComponent<ScannerController>();
            
            _diContainer.Inject(movementController);
            _diContainer.Inject(fightController);
            _diContainer.Inject(skillsController);
            _diContainer.Inject(scannerController);
            _diContainer.Inject(_player.GetComponentInChildren<FirstPersonLook>());
            
            scannerController.Init();
            
            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeHp);
            // TODO настроить сейвы
            _playerStabilitySystem.Init(1);
            _playerStabilitySystem.SetStability(100);


            
            movementController.Init(_camera);
            skillsController.Init(_camera);
            SetupCooldownListeners(skillsController);
            // TODO настроить сейвы
            var currentSave = _playerDataInteractor.CurrentSave;
            if (currentSave != null && currentSave.Position != default)
            {
                // Запускаем корутину или просто меняем позицию через задержку.
                // Так как это не MonoBehaviour, используем сам Player для корутины или Invoke.
                var monoBehaviour = _player.GetComponent<MonoBehaviour>();
                monoBehaviour.Invoke(nameof(SetPos), 0.05f);
            }
            EventBus.Raise(new PlayerSpawnEvent()
            {
                PlayerTransform = _player.transform,
                Camera = _camera
            });
            return _player;
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
            if (_player != null && _playerDataInteractor != null)
            {
                _player.transform.position = _playerDataInteractor.CurrentSave.Position;
            }
        }
    }
}