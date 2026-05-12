using Core.Audio;
using Core.Input;
using Core.Scenes;
using Gameplay.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Bootstrap
{
    [DefaultExecutionOrder(-1000)]
    public class Services : MonoBehaviour
    {
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AudioMixer _mainMixer;
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
        }
    }
}
