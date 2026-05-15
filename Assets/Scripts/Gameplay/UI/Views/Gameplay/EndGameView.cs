using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class EndGameView : View
    {
        [SerializeField] private Button restartButton;
        
        public void AddRestartButtonListener(Action callback)
        {
            restartButton.onClick.AddListener(callback.Invoke);
        }
    }
}