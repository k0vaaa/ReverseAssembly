using Core.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class CooldownView : View
    {
        [SerializeField] private Image MeleeMask;
        [SerializeField] private Image MeleeStatus;
        [SerializeField] private Image SpellMask;
        [SerializeField] private Image SpellStatus;

        private readonly UnityEvent _updateMelee = new();
        private readonly UnityEvent _updateSpell = new();

        private void Update()
        {
            _updateMelee.Invoke();
            _updateSpell.Invoke();
            //MeleeStatus.fillAmount = _skillsController.Skills[SkillType.Melee].GetReadyPercent();
            //SpellStatus.fillAmount = _skillsController.Skills[SkillType.Fireball].GetReadyPercent();
            MeleeMask.enabled = MeleeStatus.fillAmount > 0;
            SpellMask.enabled = SpellStatus.fillAmount > 0;
        }
        
        public void SetMeleeListener(UnityAction callback)
        {
            _updateMelee.AddListener(callback);
        }
        
        public void SetSpellListener(UnityAction callback)
        {
            _updateSpell.AddListener(callback);
        }

        public void SetMeleeFillAmount(float value)
        {
            MeleeStatus.fillAmount = value;
        }
        
        public void SetSpellFillAmount(float value)
        {
            SpellStatus.fillAmount = value;
        }
    }
}