using Core.UI;
using DG.Tweening;
using Plugins.Demigiant.DOTween.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class StabilityBarView : View
    {
        [SerializeField] private Image _hpFront;
        [SerializeField] private Image _hpMiddle;

        public void ChangeHp(float hp, float maxHp)
        {
            float percent = Mathf.Clamp(hp / maxHp, 0f, 1f);
            _hpFront.fillAmount = percent;
            _hpMiddle.DOFillAmount(percent, .9f);
        }
    }
}