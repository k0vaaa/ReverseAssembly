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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            onHide?.Invoke();
        }
    }
}