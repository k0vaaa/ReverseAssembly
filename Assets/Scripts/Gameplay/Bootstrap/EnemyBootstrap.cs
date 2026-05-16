
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Gameplay.Enemies;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class EnemyBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;

        [Header("Enemies")] [SerializeField] private GameObject[] _enemies;
        [SerializeField] private Vector2 _enemiesSpawnAreaExtents;
        [SerializeField] private int _enemiesCount;
        [SerializeField] private Transform _enemiesSpawnPoint;

        [Header("Boss")] 
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private Vector3 _bossSpawnPoint;

        private CharacterController _playerController;

        public void Boot()
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
