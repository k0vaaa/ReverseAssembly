using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay.HUD
{
    public class EndGameView : View
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _quitButton;

        public event Action MenuClicked;

        private void Awake()
        {
            _menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
            _quitButton.onClick.AddListener(Application.Quit);
        }
    }
}