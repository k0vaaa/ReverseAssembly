using System;
using System.Collections.Generic;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.Saveables;
using Gameplay.Combat.Health;
using Gameplay.Combat.Interfaces;
using Gameplay.Controllers.Player;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Attributes;
using UnityEngine;
using Object = UnityEngine.Object; 
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class EnemyManager
    {
        private CharacterController _player;
        [Inject] private  SettingsInteractor _settingsInteractor;
        private readonly GameObject[] _enemyPrefabs;
        private readonly Vector2 _enemiesSpawnAreaExtents;
        private readonly Vector3 _enemiesSpawnAreaCenter;
        private readonly int _enemiesCount;
        private readonly List<ICharacterController> _characters = new();
        private readonly GameObject _bossPrefab;
        private readonly Vector3 _bossSpawnPoint;
        private bool _bossSpawned;
        private int _aliveCount;

        public EnemyManager(SettingsInteractor settingsInteractor, Vector2 enemiesSpawnAreaSize,
            GameObject[] enemyPrefabs,
            Vector3 enemiesSpawnAreaCenter, int enemiesCount, GameObject bossPrefab, Vector3 bossSpawnPoint,
            CharacterController player)
        {
            _settingsInteractor = settingsInteractor ?? throw new ArgumentNullException(nameof(settingsInteractor));
            _enemiesSpawnAreaExtents = enemiesSpawnAreaSize;
            _enemyPrefabs = enemyPrefabs ?? throw new ArgumentNullException(nameof(enemyPrefabs));
            _enemiesSpawnAreaCenter = enemiesSpawnAreaCenter;
            _enemiesCount = enemiesCount;
            _bossPrefab = bossPrefab ?? throw new ArgumentNullException(nameof(bossPrefab));
            _bossSpawnPoint = bossSpawnPoint;
            _player = player;
            SpawnEnemies(_enemiesCount);
        }

        public void SpawnEnemies(int enemiesCount)
        {
            _aliveCount = enemiesCount;
            var loadSettings = _settingsInteractor.LoadSettings();
            var enemiesPower = loadSettings.EnemiesPower;
            var peaceMode = loadSettings.PeaceMode;
            for (int i = 0; i < enemiesCount; i++)
            {
                int prefabIndex = Random.Range(0, _enemyPrefabs.Length);
                var enemyPrefab = _enemyPrefabs[prefabIndex];
                var enemy = Object.Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity)
                    .GetComponent<EnemyController>();
                if (enemy == null)
                {
                    Debug.LogError($"Enemy prefab {enemyPrefab.name} does not have EnemyController component.");
                    continue;
                }

                var stabilitySystem = enemy.GetComponent<StabilitySystem>();
                if (stabilitySystem == null)
                {
                    Debug.LogError($"Enemy {enemy.name} does not have HealthSystem component.");
                    Object.Destroy(enemy.gameObject);
                    continue;
                }

                var healthBarView = enemy.GetComponentInChildren<StabilityBarView>();
                if (healthBarView == null)
                {
                    Debug.LogWarning($"Enemy {enemy.name} does not have HealthBarView component.");
                }
                else
                {
                    stabilitySystem.onStabilityChanged.AddListener(healthBarView.ChangeValue);
                }

                var skillsController = enemy.GetComponent<AbilitiesController>();
                if (skillsController == null)
                {
                    Debug.LogWarning($"Enemy {enemy.name} does not have SkillsController component.");
                }

                enemy.Init(peaceMode, _player.transform);
                stabilitySystem.Init(enemiesPower);
                stabilitySystem.OnDeath.AddListener(OnEnemyDeath);
                //skillsController?.Init(null);
                enemy.UniqueId = Guid.NewGuid().ToString();
                enemy.PrefabIndex = prefabIndex;
                _characters.Add(enemy);
            }
        }

        public List<EnemyData> GetEnemyData()
        {
            List<EnemyData> enemyDataList = new();
            foreach (ICharacterController character in _characters)
            {
                if (character != null && !character.isDead)
                {
                    var stabilitySystem = character.GetComponent<StabilitySystem>();
                    if (stabilitySystem == null)
                    {
                        Debug.LogWarning($"Character {character.UniqueId} does not have HealthSystem component.");
                        continue;
                    }

                    enemyDataList.Add(new EnemyData
                    {
                        Id = character.UniqueId,
                        Position = character.transform.position,
                        Health = stabilitySystem.Stability,
                        PrefabIndex = character is BossController ? _enemyPrefabs.Length : character.PrefabIndex,
                        IsBoss = character is BossController
                    });
                }
            }

            return enemyDataList;
        }

        public void LoadEnemies(List<EnemyData> enemyDataList)
        {
            if (enemyDataList == null || enemyDataList.Count == 0) return;

            foreach (var character in _characters)
            {
                if (character != null)
                {
                    Object.Destroy(character.gameObject);
                }
            }

            _characters.Clear();
            _aliveCount = enemyDataList.Count;

            var loadSettings = _settingsInteractor.LoadSettings();
            var enemiesPower = loadSettings.EnemiesPower;
            foreach (var enemyData in enemyDataList)
            {
                if (enemyData.PrefabIndex < 0 || enemyData.PrefabIndex >= _enemyPrefabs.Length && !enemyData.IsBoss)
                {
                    Debug.LogWarning($"Invalid PrefabIndex: {enemyData.PrefabIndex} for non-boss enemy.");
                    _aliveCount--;
                    continue;
                }

                ICharacterController character;
                GameObject instantiatedObject;
                if (enemyData.IsBoss)
                {
                    instantiatedObject = Object.Instantiate(_bossPrefab, enemyData.Position, Quaternion.identity);
                    character = instantiatedObject.GetComponent<BossController>();
                }
                else
                {
                    var enemyPrefab = _enemyPrefabs[enemyData.PrefabIndex];
                    instantiatedObject = Object.Instantiate(enemyPrefab, enemyData.Position, Quaternion.identity);
                    character = instantiatedObject.GetComponent<EnemyController>();
                }

                if (character == null)
                {
                    Debug.LogError(
                        $"Instantiated object {instantiatedObject.name} does not have required controller component.");
                    Object.Destroy(instantiatedObject);
                    _aliveCount--;
                    continue;
                }

                var healthSystem = character.GetComponent<StabilitySystem>();
                if (healthSystem == null)
                {
                    Debug.LogError($"Character {instantiatedObject.name} does not have HealthSystem component.");
                    Object.Destroy(instantiatedObject);
                    _aliveCount--;
                    continue;
                }
                

                var skillsController = instantiatedObject.GetComponent<AbilitiesController>();
                if (skillsController == null)
                {
                    Debug.LogWarning($"Character {instantiatedObject.name} does not have SkillsController component.");
                }

                character.UniqueId = enemyData.Id;
                character.PrefabIndex = enemyData.PrefabIndex;
                character.Init(loadSettings.PeaceMode, _player.transform);
                healthSystem.Init(enemiesPower);
                healthSystem.OnDeath.AddListener(OnEnemyDeath);
                healthSystem.SetStability(enemyData.Health);
                //skillsController?.Init(null);
                _characters.Add(character);
            }
        }

        private void OnEnemyDeath()
        {
            if (--_aliveCount <= 0)
            {
                Debug.Log("All characters are dead");
                if (!_bossSpawned)
                {
                    SpawnBoss();
                }
                else
                {
                    Debug.Log("Boss is dead. Game over or proceed to next stage.");
                    // Здесь можно добавить логику завершения игры или перехода к следующему этапу
                }
            }
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

        private void SpawnBoss()
        {
            if (_bossSpawned) return;
            _bossSpawned = true;
            _aliveCount = 1;

            var boss = Object.Instantiate(_bossPrefab, _bossSpawnPoint, Quaternion.identity)
                .GetComponent<BossController>();
            if (boss == null)
            {
                Debug.LogError($"Boss prefab {_bossPrefab.name} does not have BossController component.");
                _bossSpawned = false;
                _aliveCount = 0;
                return;
            }

            var healthSystem = boss.GetComponent<StabilitySystem>();
            if (healthSystem == null)
            {
                Debug.LogError($"Boss {boss.name} does not have HealthSystem component.");
                Object.Destroy(boss.gameObject);
                _bossSpawned = false;
                _aliveCount = 0;
                return;
            }

            var healthBarView = boss.GetComponentInChildren<StabilityBarView>();
            if (healthBarView == null)
            {
                Debug.LogWarning($"Boss {boss.name} does not have HealthBarView component.");
            }
            else
            {
                healthSystem.onStabilityChanged.AddListener(healthBarView.ChangeValue);
            }

            var skillsController = boss.GetComponent<AbilitiesController>();
            if (skillsController == null)
            {
                Debug.LogWarning($"Boss {boss.name} does not have SkillsController component.");
            }

            healthSystem.Init(_settingsInteractor.LoadSettings().EnemiesPower);
            healthSystem.OnDeath.AddListener(OnEnemyDeath);
            //skillsController?.Init(null);
            boss.Init(_settingsInteractor.LoadSettings().PeaceMode, _player.transform);
            boss.UniqueId = Guid.NewGuid().ToString();
            boss.PrefabIndex = -1; // Босс не использует индекс префаба из _enemyPrefabs
            _characters.Add(boss);
        }
    }
}