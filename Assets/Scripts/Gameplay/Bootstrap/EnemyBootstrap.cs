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
            var enemyManager = new EnemyManager(
                _settingsInteractor,
                _enemiesSpawnAreaExtents,
                _enemies,
                _enemiesSpawnPoint.position,
                _enemiesCount,
                _bossPrefab, 
                _bossSpawnPoint,
                _playerController);

            _saveManager.SetEnemyManager(enemyManager);

            var save = _playerDataInteractor.CurrentSave;
            if (save != null && save.Enemies != null && save.Enemies.Count > 0)
            {
                enemyManager.LoadEnemies(save.Enemies);
            }
        }
    }
}
