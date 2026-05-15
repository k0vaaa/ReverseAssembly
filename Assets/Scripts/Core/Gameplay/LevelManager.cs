using Core.DI;
using Core.Input;
using Core.Pause;
using UnityEngine;

namespace Core.Gameplay
{
    public class LevelManager : MonoBehaviour, IInjectable
    {
        [Inject] private InputManager _inputManager;
        
        private void OnEnable()
        {
            _inputManager.OnEscapePressed += TogglePause;
        }
        private void OnDisable()
        {
            _inputManager.OnEscapePressed -= TogglePause;
        }

        private void TogglePause()
        {
            PauseManager.TogglePause();
        }
        
    }
}