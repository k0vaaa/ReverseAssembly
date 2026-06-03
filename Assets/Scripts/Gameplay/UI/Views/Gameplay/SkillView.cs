using System;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private Image _mask;
        [SerializeField] private Image _status;
        
        private readonly UnityEvent _updateSlot = new();

        private void Update()
        {
            _mask.enabled = _status.fillAmount > 0;
            if (_mask.enabled)
            {
                _updateSlot.Invoke();
            }
        }

        public void SetFill(float value)
        {
            _status.fillAmount = value;
        }
    }
}