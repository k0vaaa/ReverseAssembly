
using System;
using Core.Events;
using Core.Inventory;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.Events;
using Gameplay.UI;
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
        private Container _playerContainer;

        public void Boot()
        {
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            Player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
            new HudSwitcher(_viewManager.GetView<PlayerHUDView>());
            _playerStabilitySystem = Player.GetComponent<StabilitySystem>();
            var abilitiesController = Player.GetComponent<AbilitiesController>();
            var movementController = Player.GetComponent<MovementController>();
            var fightController = Player.GetComponent<FightController>();
            var brain = Player.GetComponent<PlayerBrain>();
            _camera = Player.GetComponentInChildren<Camera>();
            _playerContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(movementController);
                builder.RegisterValue(fightController);
                builder.RegisterValue(abilitiesController);
                builder.RegisterValue(_camera);
                builder.RegisterValue(brain);
            });
            GameObjectInjector.InjectRecursive(Player,_playerContainer);
            brain.Init();
            abilitiesController.Init();

            _stabilityBarView = _viewManager.GetView<StabilityBarView>();
            _cooldownView = _viewManager.GetView<CooldownView>();
            
            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeHp);
            
            // TODO настроить сейвы
            _playerStabilitySystem.Init(1);
            _playerStabilitySystem.SetStability(100);


            
            //skillsController.Init(_camera);
            SetupCooldownListeners(abilitiesController);
            // TODO настроить сейвы
            var currentSave = _playerDataInteractor.CurrentSave;
            if (currentSave != null && currentSave.Position != default)
            {
                // Запускаем корутину или просто меняем позицию через задержку.
                // Так как это не MonoBehaviour, используем сам Player для корутины или Invoke.
                var monoBehaviour = Player.GetComponent<MonoBehaviour>();
                monoBehaviour.Invoke(nameof(SetPos), 0.05f);
            }
            Player.SetActive(true);
            EventBus.Raise(new PlayerSpawnEvent()
            {
                PlayerTransform = Player.transform,
                Camera = _camera
            });
            
        }

        private void SetupCooldownListeners(AbilitiesController abilitiesController)
        {
            _cooldownView.SetSlot1Listener(() =>
                _cooldownView.SetSlot1FillAmount(abilitiesController.TryGetSkill<ScannerSkill>().GetReadyPercent()));
            _cooldownView.SetSlot2Listener(() =>
                _cooldownView.SetSlot2FillAmount(abilitiesController.TryGetSkill<SwitchBranchSkill>().GetReadyPercent()));
            _cooldownView.SetSlot3Listener(() =>
                _cooldownView.SetSlot3FillAmount(abilitiesController.TryGetSkill<MeleeSkill>().GetReadyPercent()));
            _cooldownView.SetSlot4Listener(() =>
                _cooldownView.SetSlot4FillAmount(abilitiesController.TryGetSkill<ProjectileSkill>().GetReadyPercent()));
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
        }
    }
}