using Core.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Core.Events;
using Core.Extensions;
using Gameplay.Events;

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