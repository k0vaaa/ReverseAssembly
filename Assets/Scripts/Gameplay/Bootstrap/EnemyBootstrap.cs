using Core.DI;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class EnemyBootstrap
    {
        private SettingsInteractor _settingsInteractor;
        private PlayerDataInteractor _playerDataInteractor;

        private readonly GameObject[] _enemies;
        private readonly Vector2 _enemiesSpawnAreaExtents;
        private readonly int _enemiesCount;
        private readonly Transform _enemiesSpawnPoint;
        
        private readonly GameObject _bossPrefab;
        private readonly Vector3 _bossSpawnPoint;
        private readonly CharacterController _playerController;

        public EnemyBootstrap(
            SettingsInteractor settingsInteractor,
            PlayerDataInteractor playerDataInteractor,
            GameObject[] enemies,
            Vector2 enemiesSpawnAreaExtents,
            int enemiesCount,
            Transform enemiesSpawnPoint,
            GameObject bossPrefab,
            Vector3 bossSpawnPoint,
            CharacterController playerController)
        {
            _settingsInteractor = settingsInteractor;
            _playerDataInteractor = playerDataInteractor;
            _enemies = enemies;
            _enemiesSpawnAreaExtents = enemiesSpawnAreaExtents;
            _enemiesCount = enemiesCount;
            _enemiesSpawnPoint = enemiesSpawnPoint;
            _bossPrefab = bossPrefab;
            _bossSpawnPoint = bossSpawnPoint;
            _playerController = playerController;

        }

        public void SetupEnemies()
        {
            var enemyManager = new EnemyManager(
                _settingsInteractor,
                _enemiesSpawnAreaExtents,
                _enemies,
                _enemiesSpawnPoint.position,
                _enemiesCount,
                _bossPrefab, 
                _bossSpawnPoint,
                _playerController);

            enemyManager.LoadEnemies(_playerDataInteractor.CurrentSave.Enemies);
        }
    }
}
