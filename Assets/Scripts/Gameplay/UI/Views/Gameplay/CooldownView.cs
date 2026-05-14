using Core.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class CooldownView : View
    {
        [SerializeField] private Image Slot1Mask;
        [SerializeField] private Image Slot1Status;
        [SerializeField] private Image Slot2Mask;
        [SerializeField] private Image Slot2Status;

        private readonly UnityEvent _updateSlot1 = new();
        private readonly UnityEvent _updateSlot2 = new();

        private void Update()
        {
            _updateSlot1.Invoke();
            _updateSlot2.Invoke();
     
            Slot1Mask.enabled = Slot1Status.fillAmount > 0;
            Slot2Mask.enabled = Slot2Status.fillAmount > 0;
        }

        public void SetSlot1Listener(UnityAction callback)
        {
            _updateSlot1.AddListener(callback);
        }

        public void SetSlot2Listener(UnityAction callback)
        {
            _updateSlot2.AddListener(callback);
        }

        public void SetSlot1FillAmount(float value)
        {
            Slot1Status.fillAmount = value;
        }

        public void SetSlot2FillAmount(float value)
        {
            Slot2Status.fillAmount = value;
        }
    }
}