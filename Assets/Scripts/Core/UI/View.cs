using UnityEngine;
using UnityEngine.Events;

namespace Core.UI
{
    public abstract class View : MonoBehaviour
    {
        public UnityEvent onShow = new();
        public UnityEvent onHide = new();

        public void Show()
        {
            gameObject.SetActive(true);
            onShow?.Invoke();
            OnShow();
        }

        protected virtual void OnShow()
        {
            
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            onHide?.Invoke();
            OnHide();
        }
        protected virtual void OnHide()
        {
            
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
            if (isVisible)
            {
                onShow?.Invoke();
            }
            else
            {
                onHide?.Invoke();
            }
        }
    }
}