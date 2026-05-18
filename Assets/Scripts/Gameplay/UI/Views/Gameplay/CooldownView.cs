using Core.UI;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Core.Events;
using Core.Extensions;
using Gameplay.Events; 
namespace Gameplay.UI.Views.Gameplay
{
    public class CooldownView : View
    {
        [SerializeField] private Image Slot1Mask;
        [SerializeField] private Image Slot1Status;
        [SerializeField] private Image Slot2Mask;
        [SerializeField] private Image Slot2Status;
        
        // private SkillsController _skillsController;
        // public void Init(SkillsController controller)
        // {
        //     _skillsController = controller;
        // }
        
        // private void OnEnable()
        // {
        //     EventBus.Subscribe<SkillCooldownChangedEvent>(OnCooldownChanged).AddTo(gameObject);
        // }
        // private void OnCooldownChanged(SkillCooldownChangedEvent e)
        // {
        //     if (e.SkillType == SkillType.BranchSwitch) 
        //         Slot1Status.fillAmount = e.ReadyPercent;
        //     
        //     if (e.SkillType == SkillType.Scanner) 
        //         Slot2Status.fillAmount = e.ReadyPercent;
        // }
        
        private readonly UnityEvent _updateSlot1 = new();
        private readonly UnityEvent _updateSlot2 = new();

        private void Update()
        {
            _updateSlot1.Invoke();
            _updateSlot2.Invoke();
     
            Slot1Mask.enabled = Slot1Status.fillAmount > 0;
            Slot2Mask.enabled = Slot2Status.fillAmount > 0;
            
            // if (_skillsController == null) return;
            //
            // // Обновляем оба слота, если они есть
            // if (Slot1Status != null)
            //     Slot1Status.fillAmount = _skillsController.GetSkillReadyPercent(SkillType.BranchSwitch);
            //
            // if (Slot2Status != null)
            //     Slot2Status.fillAmount = _skillsController.GetSkillReadyPercent(SkillType.Scanner);
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