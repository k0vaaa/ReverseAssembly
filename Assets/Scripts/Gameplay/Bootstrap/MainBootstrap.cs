using Core.Audio;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Repos;
using Core.Scenes;
using Core.UI;
using Core.Utilities;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class MainBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [Inject] private InputManager _inputManager;
        [Inject] private ViewManager _viewManager;
        private AudioManager _audioManager;
        private IPlayerDataRepository _playerJsonRepository;
        private IDataRepository _playerPrefsRepository;
        private PlayerDataInteractor _playerDataInteractor;
        private SettingsInteractor _settingsInteractor;
        public void Boot()
        {
            
        
        }
    }
}