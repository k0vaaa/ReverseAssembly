using Core.UI;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.UI
{
    public class ScannerView : View
    {
        [SerializeField] private RectTransform _mask;
        [SerializeField] private float _fillDuration = .5f;
        [SerializeField] private Vector2 _maskSize = new Vector2(2600, 2600);
        public void FillIn()
        {
            _mask.DOSizeDelta(_maskSize, _fillDuration);
        }

        public void FillOut()
        {
            _mask.DOSizeDelta(Vector2.zero, _fillDuration);
        }
    }
}
