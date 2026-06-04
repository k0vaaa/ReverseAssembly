using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay.Terminal
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableCodeLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void DragAction();
        public event DragAction OnDragEnded;

        private Transform _parent;
        private GameObject _placeholder;
        private CanvasGroup _canvasGroup;
        private LayoutElement _layoutElement;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _layoutElement = GetComponent<LayoutElement>();
            if (_layoutElement == null)
            {
                _layoutElement = gameObject.AddComponent<LayoutElement>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _parent = transform.parent;
            
            // Создаем плейсхолдер, чтобы остальные элементы не "схлопнулись"
            _placeholder = new GameObject("Placeholder");
            var rect = _placeholder.AddComponent<RectTransform>();
            var myRect = GetComponent<RectTransform>();
            rect.sizeDelta = myRect.sizeDelta;
            
            var le = _placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = _layoutElement.preferredWidth > 0 ? _layoutElement.preferredWidth : myRect.sizeDelta.x;
            le.preferredHeight = _layoutElement.preferredHeight > 0 ? _layoutElement.preferredHeight : myRect.sizeDelta.y;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;

            _placeholder.transform.SetParent(_parent, false);
            _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            _layoutElement.ignoreLayout = true;
            _canvasGroup.blocksRaycasts = false; // Чтобы луч мыши пробивал этот объект и попадал в другие
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Корректный перевод координат экрана в мировые координаты (работает для WorldSpace, ScreenSpace Camera/Overlay)
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)_parent, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
            {
                transform.position = globalMousePos;
            }

            int newSiblingIndex = _parent.childCount - 1; // По умолчанию в конец

            for (int i = 0; i < _parent.childCount; i++)
            {
                Transform sibling = _parent.GetChild(i);
                if (sibling == this.transform || sibling == _placeholder.transform) continue;

                // Для VerticalLayoutGroup элементы идут сверху вниз (Y уменьшается)
                // Сравниваем в мировых координатах (так как transform.position теперь тоже в мировых)
                if (transform.position.y > sibling.position.y)
                {
                    newSiblingIndex = sibling.GetSiblingIndex();
                    // Корректируем индекс, если плейсхолдер уже стоит перед ним
                    if (_placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    {
                        newSiblingIndex--; 
                    }
                    break;
                }
            }

            _placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _layoutElement.ignoreLayout = false;
            _canvasGroup.blocksRaycasts = true;
            
            transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());
            Destroy(_placeholder);
            
            OnDragEnded?.Invoke();
        }
    }
}
