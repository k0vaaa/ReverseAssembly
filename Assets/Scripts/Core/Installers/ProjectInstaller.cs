using System;
using Core.Audio;
using Core.Bootstrap;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Repos;
using Core.Scenes;
using Core.Static;
using Reflex.Core;
using Reflex.Enums;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.Audio;
using Resolution = Reflex.Enums.Resolution;

namespace Core.Installers
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private AudioManager _audioManagerPrefab;
        private IPlayerDataRepository _playerJsonRepository;
        private IDataRepository _playerPrefsRepository;
        private PlayerDataInteractor _playerDataInteractor;
        private SettingsInteractor _settingsInteractor;
        private SceneLoader _sceneLoader;
        private InputManager _inputManager;


        public void InstallBindings(ContainerBuilder builder)
        {
            AudioManager audioManager = Instantiate(_audioManagerPrefab);
            DontDestroyOnLoad(audioManager);
            builder.RegisterValue(audioManager);
            
            builder.RegisterValue(new JsonRepository(Constants.SAVE_FILE_NAME), new []{typeof(IPlayerDataRepository)});
            builder.RegisterType(typeof(PlayerPrefsRepository),new []{typeof(IDataRepository)}, Lifetime.Singleton, Resolution.Eager);

            builder.RegisterType(typeof(SettingsInteractor), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(PlayerDataInteractor), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(SceneLoader), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(InputManager), new []{typeof(InputManager), typeof(IInitializable)}, Lifetime.Singleton, Resolution.Eager);

            
        }
    }
}