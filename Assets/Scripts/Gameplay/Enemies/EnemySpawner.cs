using System;
using Core.SaveLoad.Interactors;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class EnemySpawner
    {
        private readonly EnemyRegistry _registry;
        private readonly SettingsInteractor _settingsInteractor;
        private readonly CharacterController _player;
        
        private readonly GameObject[] _enemyPrefabs;
        private readonly Vector2 _enemiesSpawnAreaExtents;
        private readonly Vector3 _enemiesSpawnAreaCenter;
        
        private readonly GameObject _bossPrefab;
        private readonly Vector3 _bossSpawnPoint;

        public EnemySpawner(
            EnemyRegistry registry, 
            SettingsInteractor settingsInteractor, 
            CharacterController player,
            GameObject[] enemyPrefabs,
            Vector2 enemiesSpawnAreaExtents,
            Vector3 enemiesSpawnAreaCenter,
            GameObject bossPrefab,
            Vector3 bossSpawnPoint)
        {
            _registry = registry;
            _settingsInteractor = settingsInteractor;
            _player = player;
            _enemyPrefabs = enemyPrefabs;
            _enemiesSpawnAreaExtents = enemiesSpawnAreaExtents;
            _enemiesSpawnAreaCenter = enemiesSpawnAreaCenter;
            _bossPrefab = bossPrefab;
            _bossSpawnPoint = bossSpawnPoint;
        }

        public void SpawnRandomEnemies(int count)
        {
            if (count <= 0) return;
            var loadSettings = _settingsInteractor.LoadSettings();
            var enemiesPower = loadSettings.EnemiesPower;
            var peaceMode = loadSettings.PeaceMode;
            
            for (int i = 0; i < count; i++)
            {
                int prefabIndex = Random.Range(0, _enemyPrefabs.Length);
                var enemyPrefab = _enemyPrefabs[prefabIndex];
                
                var enemy = Object.Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity)
                    .GetComponent<AIController>();
                    
                if (enemy == null)
                {
                    Debug.LogError($"Enemy prefab {enemyPrefab.name} does not have AIController component.");
                    continue;
                }

                enemy.Init(peaceMode, _player.transform);
                enemy.StabilitySystem.Init(enemiesPower);
                
                enemy.UniqueId = Guid.NewGuid().ToString();
                enemy.PrefabIndex = prefabIndex;
                
                _registry.Register(enemy);
            }
        }

        public void SpawnBoss()
        {
            if (_bossPrefab == null) return;
            
            var boss = Object.Instantiate(_bossPrefab, _bossSpawnPoint, Quaternion.identity)
                .GetComponent<AIController>();
                
            if (boss == null) return;

            boss.UniqueId = Guid.NewGuid().ToString();
            boss.PrefabIndex = -1;
            
            boss.Init(_settingsInteractor.LoadSettings().PeaceMode, _player.transform);
            boss.StabilitySystem.Init(_settingsInteractor.LoadSettings().EnemiesPower);
            
            // Add a field or property if you want to explicitly tag the boss, 
            // but its strategy or prefab dictates its behaviour.
            
            _registry.Register(boss);
        }
        
        public AIController SpawnEnemyByPrefabIndex(int prefabIndex, Vector3 position)
        {
            var enemyPrefab = _enemyPrefabs[prefabIndex];
            var enemy = Object.Instantiate(enemyPrefab, position, Quaternion.identity)
                .GetComponent<AIController>();
            return enemy;
        }
        
        public AIController SpawnBossAt(Vector3 position)
        {
            var boss = Object.Instantiate(_bossPrefab, position, Quaternion.identity)
                .GetComponent<AIController>();
            return boss;
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(_enemiesSpawnAreaCenter.x - _enemiesSpawnAreaExtents.x,
                    _enemiesSpawnAreaCenter.x + _enemiesSpawnAreaExtents.x),
                _enemiesSpawnAreaCenter.y,
                Random.Range(_enemiesSpawnAreaCenter.z - _enemiesSpawnAreaExtents.y,
                    _enemiesSpawnAreaCenter.z + _enemiesSpawnAreaExtents.y));
        }
    }
}
