
using Core.Input;
using Core.Pause;
using Reflex.Attributes;
using UnityEngine;

namespace Core.Gameplay
{
    public class KeyBindingManager : MonoBehaviour
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