using System.Collections.Generic;
using System.Linq;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.Saveables;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemySaveHandler
    {
        private readonly EnemyRegistry _registry;
        private readonly EnemySpawner _spawner;
        private readonly BossDirector _bossDirector;
        private readonly SettingsInteractor _settingsInteractor;
        private readonly CharacterController _player;

        public EnemySaveHandler(
            EnemyRegistry registry, 
            EnemySpawner spawner, 
            BossDirector bossDirector,
            SettingsInteractor settingsInteractor,
            CharacterController player)
        {
            _registry = registry;
            _spawner = spawner;
            _bossDirector = bossDirector;
            _settingsInteractor = settingsInteractor;
            _player = player;
        }

        public List<EnemyData> GetSaveData()
        {
            List<EnemyData> enemyDataList = new();
            foreach (var character in _registry.Characters)
            {
                if (character != null && !character.isDead && character.StabilitySystem.Stability > 0)
                {
                    enemyDataList.Add(new EnemyData
                    {
                        Id = character.UniqueId,
                        Position = character.Mover != null ? character.transform.position : character.transform.position,
                        Health = character.StabilitySystem.Stability,
                        PrefabIndex = character.PrefabIndex,
                        IsBoss = character.PrefabIndex == -1 && character.CompareTag("Boss") // Optional check if needed
                    });
                }
            }
            return enemyDataList;
        }

        public void RestoreFromSave(List<EnemyData> enemyDataList)
        {
            if (enemyDataList == null) return;

            var savedIds = new HashSet<string>(enemyDataList.Select(e => e.Id));
            
            _registry.ClearMissing();

            // 1. Убираем тех, кого нет в сейве (убиты)
            var charactersArray = _registry.Characters.ToArray();
            foreach (var character in charactersArray)
            {
                if (character != null && !savedIds.Contains(character.UniqueId))
                {
                    _registry.Unregister(character);
                    Object.Destroy(character.gameObject);
                }
            }

            var loadSettings = _settingsInteractor.LoadSettings();
            var enemiesPower = loadSettings.EnemiesPower;
            var peaceMode = loadSettings.PeaceMode;

            // 2. Обновляем оставшихся и создаем тех, которых не хватает
            foreach (var enemyData in enemyDataList)
            {
                var existingEnemy = _registry.Characters
                    .FirstOrDefault(c => c != null && c.UniqueId == enemyData.Id);

                if (existingEnemy != null)
                {
                    var agent = existingEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent != null && agent.isActiveAndEnabled)
                        agent.Warp(enemyData.Position);
                    else
                        existingEnemy.transform.position = enemyData.Position;
                    
                    existingEnemy.StabilitySystem.SetStability(enemyData.Health);
                    
                    if (enemyData.IsBoss) _bossDirector.MarkBossSpawned();
                }
                else
                {
                    if (enemyData.PrefabIndex < 0 && !enemyData.IsBoss)
                    {
                        Debug.LogWarning($"Preplaced enemy {enemyData.Id} is missing from the scene. Cannot restore.");
                        continue;
                    }

                    AIController character;
                    if (enemyData.IsBoss)
                    {
                        character = _spawner.SpawnBossAt(enemyData.Position);
                        _bossDirector.MarkBossSpawned();
                    }
                    else
                    {
                        character = _spawner.SpawnEnemyByPrefabIndex(enemyData.PrefabIndex, enemyData.Position);
                    }

                    if (character == null) continue;

                    character.UniqueId = enemyData.Id;
                    character.PrefabIndex = enemyData.PrefabIndex;
                    character.Init(peaceMode, _player.transform);
                    character.StabilitySystem.Init(enemiesPower);
                    character.StabilitySystem.SetStability(enemyData.Health);
                    
                    _registry.Register(character);
                }
            }
        }
    }
}
