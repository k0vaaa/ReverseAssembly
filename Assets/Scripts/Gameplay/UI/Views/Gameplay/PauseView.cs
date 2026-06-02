using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class PauseView : View
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _loadLastButton;
        [SerializeField] private Button _toLoadChooseButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _resumeButton;

        public event Action SaveClick;
        public event Action MainMenuClick;
        public event Action LoadLastClick;
        public event Action ToLoadChooseClick;
        public event Action ExitClick;
        public event Action ResumeClick;


        private  void Awake()
        {
            _saveButton?.onClick.AddListener(() => SaveClick?.Invoke());
            _mainMenuButton?.onClick.AddListener(() => MainMenuClick?.Invoke());
            _loadLastButton?.onClick.AddListener(() => LoadLastClick?.Invoke());
            _toLoadChooseButton?.onClick.AddListener(() => ToLoadChooseClick?.Invoke());
            _exitButton?.onClick.AddListener(() => ExitClick?.Invoke());
            _resumeButton?.onClick.AddListener(() => ResumeClick?.Invoke());
        }
    }
}