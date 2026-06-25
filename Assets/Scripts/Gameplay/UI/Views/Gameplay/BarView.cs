using Core.UI;
using Plugins.Demigiant.DOTween.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class BarView : View
    {
        [SerializeField] private Image _barFront;
        [SerializeField] private Image _barMiddle;
        [SerializeField][Range(0,1)] private float _fill = 1f;

        private void OnValidate()
        {
            ChangeValue(_fill,1);
        }

        public void ChangeValue(float hp, float maxHp)
        {
            float percent = Mathf.Clamp(hp / maxHp, 0f, 1f);
            _barFront.fillAmount = percent;
            _barMiddle.DOFillAmount(percent, .9f);
        }

        public void ChangeValue(float percent)
        {
            _barFront.fillAmount = percent;
            _barMiddle.DOFillAmount(percent, .9f);
        }
    }
}