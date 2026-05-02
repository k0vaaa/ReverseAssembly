using Core.DI;
using Core.Gameplay;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Combat.Health;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using Gameplay.Enemies;
using Gameplay.UI.Views.Gameplay;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class GameplayBootstrap : MonoBehaviour,IInjectable,IInitializable
    {
        [Header("Player")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private CooldownView _cooldownView;
        [SerializeField] private StabilityBarView _stabilityBarView;
        [Inject] private InputManager _inputManager;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private ViewManager _viewManager;
        private GameObject _player;
        private StabilitySystem _playerStabilitySystem;
  


        [Header("Enemies")]
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private Vector2 _enemiesSpawnAreaExtents;
        [SerializeField] private int _enemiesCount;
        [SerializeField] private Transform _enemiesSpawnPoint;
       

        [Header("Boss")]
        [SerializeField] private GameObject _boss;
        [SerializeField] private Vector3 _bossSpawnPoint;
        

        public void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //var uiManager = new GameplayUIManager(_playerDataInteractor, viewManager);
            SetUpPlayer();
            var skillsController = _player.GetComponent<SkillsController>();
            skillsController.Init(_camera);
            _cooldownView.SetMeleeListener(() =>
                _cooldownView.SetMeleeFillAmount(skillsController.Skills[SkillType.Melee].GetReadyPercent()));
            _cooldownView.SetSpellListener(() =>
                _cooldownView.SetSpellFillAmount(skillsController.Skills[SkillType.Fireball].GetReadyPercent()));
            _bossSpawnPoint = new Vector3(323.6f,261.71f, -103.5f);
            
            var enemyManager = new EnemyManager(_settingsInteractor,
                _enemiesSpawnAreaExtents,
                _enemies,
                _enemiesSpawnPoint.position,
                _enemiesCount,
                _boss, _bossSpawnPoint,_player.GetComponent<CharacterController>());
            //gameplayManager.Init(_inputManager, _playerStabilitySystem, viewManager, _playerDataInteractor, enemyManager);
            enemyManager.LoadEnemies(_playerDataInteractor.CurrentSave.Enemies); // Восстанавливаем врагов
            
        }
        
        
        private void SetUpPlayer()
        {   
            Debug.Log(_playerDataInteractor.CurrentSave.Position);
            _player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
            Debug.Log(_player.transform.position);
            _playerStabilitySystem = _player.GetComponent<StabilitySystem>();
            // healthBarView.Init(_playerHealthSystem.onHealthChanged);
            _playerStabilitySystem.onStabilityChanged.AddListener(_stabilityBarView.ChangeHp);
            _playerStabilitySystem.Init(1);
            _player.GetComponent<MovementController>().Init(_camera);
            _playerStabilitySystem.SetStability(_playerDataInteractor.CurrentSave.Health);
      
            if (_playerDataInteractor.CurrentSave.Position == default) return;
            //_player.transform.position = _playerDataInteractor.CurrentSave.Position;
            /*Vector3 position = _playerDataInteractor.CurrentSave.Position;
            position.y += 3f;
            _player.transform.position = position;*/
            Debug.Log(_player.transform.position);
            Invoke(nameof(SetPos),.05f);
            Invoke(nameof(LogPos), 1f);
        }

        private void LogPos()
        {
            Debug.Log(_player.transform.position);
        }
        
        private void SetPos()
        {
            _player.transform.position = _playerDataInteractor.CurrentSave.Position;
        }
        
    }
}