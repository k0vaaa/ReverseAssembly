using Plugins.Demigiant.DOTween.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay
{
    public class StabilityBarView : MonoBehaviour
    {
        // [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private Image _hpFront;
        [SerializeField] private Image _hpMiddle;

        // public void Init(UnityEvent<float,float> onHealthChanged)
        // {
        //     onHealthChanged.AddListener(ChangeHp);
        // }

        private void Start()
        {
            // if(_healthSystem) _healthSystem.onHealthChanged.AddListener(ChangeHp);
        }

        public void ChangeHp(float hp, float maxHp)
        {
            float percent = Mathf.Clamp(hp / maxHp, 0f, 1f);
            _hpFront.fillAmount = percent;
            _hpMiddle.DOFillAmount(percent, .9f);
        }
    }
}