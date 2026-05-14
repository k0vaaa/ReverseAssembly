using Core.Audio;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Repos;
using Core.Scenes;
using Core.Static;
using Core.UI;
using Gameplay.Controllers.Player;
using Gameplay.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace Core.Bootstrap
{
    [DefaultExecutionOrder(-1000)]
    public class Services : MonoBehaviour
    {
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AudioMixer _mainMixer;
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private BranchManager _branchManager;
        [SerializeField] private Volume _postProcessVolume;
        private PlayerDataInteractor _playerDataInteractor;
        private IPlayerDataRepository _playerJsonRepository;
        private IDataRepository _playerPrefsRepository;
        private SettingsInteractor _settingsInteractor;
        private InputManager _inputManager;
        private SceneLoader _sceneLoader;
        private SyncEnergyManager _syncEnergyManager;

        private void Awake()
        {
            CreateServices();
        }

        private void CreateServices()
        {
            _sceneLoader = new SceneLoader();
            _inputManager = new InputManager();
            _syncEnergyManager = new SyncEnergyManager();
            _playerJsonRepository = new JsonRepository(Constants.SAVE_FILE_NAME);
            _playerPrefsRepository = new PlayerPrefsRepository();
            _playerDataInteractor = new PlayerDataInteractor(_playerJsonRepository);
            _settingsInteractor = new SettingsInteractor(_playerPrefsRepository);
        }
    }
}
