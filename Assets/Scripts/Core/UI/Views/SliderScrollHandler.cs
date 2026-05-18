using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI.Views
{
    public class SliderScrollHandler : MonoBehaviour, IScrollHandler
    {
        [SerializeField] private float _scrollSpeed = .003f;
        
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void OnScroll(PointerEventData eventData)
        {
            _slider.value += eventData.scrollDelta.y * _scrollSpeed;
        }
    }
}