using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Gameplay.Core;
using Gameplay.Enemies;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class EnemyBootstrap : BootstrapComponent
    {
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private SaveManager _saveManager;

        [Header("Enemies")] [SerializeField] private GameObject[] _enemies;
        [SerializeField] private Vector2 _enemiesSpawnAreaExtents;
        [SerializeField] private int _enemiesCount;
        [SerializeField] private Transform _enemiesSpawnPoint;

        [Header("Boss")] 
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private Vector3 _bossSpawnPoint;

        private CharacterController _playerController;

        protected override void OnBoot()
        {
            _playerController = GetComponent<PlayerBootstrap>().Player.GetComponent<CharacterController>();
            var registry = new EnemyRegistry();
            var spawner = new EnemySpawner(
                registry, 
                _settingsInteractor, 
                _playerController, 
                _enemies, 
                _enemiesSpawnAreaExtents, 
                _enemiesSpawnPoint.position, 
                _bossPrefab, 
                _bossSpawnPoint);
            var bossDirector = new BossDirector(registry, spawner);
            var saveHandler = new EnemySaveHandler(registry, spawner, bossDirector, _settingsInteractor, _playerController);

            // Ищем уже существующих на сцене врагов и регистрируем их
            var preplacedEnemies = FindObjectsByType<AIController>(FindObjectsSortMode.None);
            foreach (var enemy in preplacedEnemies)
            {
                enemy.PrefabIndex = -1; // -1 means it's manually placed
                enemy.Init(_settingsInteractor.LoadSettings().PeaceMode, _playerController.transform);
                enemy.StabilitySystem.Init(_settingsInteractor.LoadSettings().EnemiesPower);
                registry.Register(enemy);
            }

            var save = _playerDataInteractor.CurrentSave;
            if (save != null && save.Position != default && save.Enemies != null)
            {
                saveHandler.RestoreFromSave(save.Enemies);
            }
            else
            {
                // Если нет сейва, просто спавним заданное количество
                spawner.SpawnRandomEnemies(_enemiesCount);
            }

            // Передаем новый SaveHandler в SaveManager
            _saveManager.SetEnemySaveHandler(saveHandler);
        }
    }
}
