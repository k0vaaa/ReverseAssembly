using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.MainMenu
{
    public class SettingsView : View
    {
        [SerializeField] private Slider _enemiesPower;
        [SerializeField] private Button _goBackButton;
        [SerializeField] private Toggle _peaceModeToggle;
        public void SetSliderValue(float value)
        {
            _enemiesPower.value = value;
        }
        
        public void SetPeaceMode(bool value)
        {
            _peaceModeToggle.isOn = value;
        }
        
        public void SetSliderListener(Action<float> callback)
        {
            _enemiesPower.onValueChanged.AddListener(callback.Invoke);
        }
        
        public void SetPeaceModeListener(Action<bool> callback)
        {
            _peaceModeToggle.onValueChanged.AddListener(callback.Invoke);
        }

        public void SetBackButtonListener(Action callback)
        {
            _goBackButton.onClick.AddListener(callback.Invoke);
        }
    }
}