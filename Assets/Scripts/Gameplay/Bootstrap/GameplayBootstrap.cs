using Core.Bootstrap;
using Core.DI;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Controllers.Player;
using Gameplay.UI;
using Gameplay.UI.Views.Gameplay;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class GameplayBootstrap : MonoBehaviour, IInjectable
    {
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private DIContainer _diContainer;

        [Header("Player")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Camera _camera;
        [Inject] private ViewManager _viewManager;
        //[SerializeField] private CooldownView _cooldownView;
        //[SerializeField] private StabilityBarView _stabilityBarView;


        [Header("Enemies")]
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private Vector2 _enemiesSpawnAreaExtents;
        [SerializeField] private int _enemiesCount;
        [SerializeField] private Transform _enemiesSpawnPoint;

        [Header("Boss")]
        [SerializeField] private GameObject _boss;
        [SerializeField] private Vector3 _bossSpawnPoint;
        
        
        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            var hudView = _viewManager.GetView<PlayerHUDView>();
            new Gameplay.UI.HudSwitcher(hudView);
            // 1. Setup Player
            var player = SetupPlayer();

            // 2. Setup Enemies
            //SetupEnemy(player);
            
            
        }
        
        
        

        private GameObject SetupPlayer()
        {   
            
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            var playerBootstrap = new PlayerBootstrap(
                _playerDataInteractor,
                _playerSpawnPoint,
                _playerPrefab,
                _camera,
                _viewManager.GetView<CooldownView>(),
                _viewManager.GetView<StabilityBarView>(),
                _diContainer
            );
            var player = playerBootstrap.SetupPlayer();
            return player;
            
        }
        

        private void SetupEnemy(GameObject player)
        {
            _bossSpawnPoint = new Vector3(323.6f, 261.71f, -103.5f);
            var enemyBootstrap = new EnemyBootstrap(
                _settingsInteractor,
                _playerDataInteractor,
                _enemies,
                _enemiesSpawnAreaExtents,
                _enemiesCount,
                _enemiesSpawnPoint,
                _boss,
                _bossSpawnPoint,
                player.GetComponent<CharacterController>()
            );
            enemyBootstrap.SetupEnemies();
        }
    }
}
