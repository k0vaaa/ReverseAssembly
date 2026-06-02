using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.MainMenu
{
    public class MainMenuView : View
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(Application.Quit);
        }

        public void SetResumeAction(Action callback)
        {
            _resumeButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.AddListener(callback.Invoke);
        }
        
        public void SetNewGameAction(Action callback)
        {
            _newGameButton.onClick.RemoveAllListeners();
            _newGameButton.onClick.AddListener(callback.Invoke);
        }

        public void SetLoadAction(Action callback)
        {
            _loadButton.onClick.RemoveAllListeners();
            _loadButton.onClick.AddListener(callback.Invoke);
        }

        public void SetSettingsAction(Action callback)
        {
            _settingsButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.AddListener(callback.Invoke);
        }
    }
}