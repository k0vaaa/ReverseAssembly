using Core.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class CooldownView : View
    {
        [SerializeField] private SkillView _slot1;
        [SerializeField] private SkillView _slot2;
        [SerializeField] private SkillView _slot3;
        [SerializeField] private SkillView _slot4;


        public void SetSlot1(SkillView view) => _slot1 = view;
        public void SetSlot2(SkillView view) => _slot2 = view;
        public void SetSlot3(SkillView view) => _slot3 = view;
        public void SetSlot4(SkillView view) => _slot4 = view;
        
        public void SetSlot1FillAmount(float value) => _slot1.SetFill(value);
        public void SetSlot2FillAmount(float value) => _slot2.SetFill(value);
        public void SetSlot3FillAmount(float value) => _slot3.SetFill(value);
        public void SetSlot4FillAmount(float value) => _slot4.SetFill(value);

        public void ResetAll()
        {
            _slot1.SetFill(0);
            _slot2.SetFill(0);
            _slot3.SetFill(0);
            _slot4.SetFill(0);
        }


    }
}