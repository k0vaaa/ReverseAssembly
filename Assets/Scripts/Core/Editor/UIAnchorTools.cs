using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public abstract class UIAnchorTools
    {
        // Хоткей: Ctrl + ] (или Cmd + ] на Mac)
        [MenuItem("Tools/UI/Anchors to Corners %]")]
        static void AnchorsToCorners()
        {
            foreach (Transform transform in Selection.transforms)
            {
                RectTransform t = transform as RectTransform;
                RectTransform pt = transform.parent as RectTransform;

                if (t == null || pt == null) continue;

                // Регистрируем действие для Ctrl+Z (Отмена)
                Undo.RecordObject(t, "Snap Anchors to Corners");

                // Высчитываем новые позиции якорей
                Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
                Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

                // Применяем якоря
                t.anchorMin = newAnchorsMin;
                t.anchorMax = newAnchorsMax;
            
                // Обнуляем отступы (теперь якоря держат объект)
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }
        [MenuItem("Tools/UI/Anchors to Center %[")]
        static void AnchorsToCenter()
        {
            foreach (Transform transform in Selection.transforms)
            {
                if (transform is RectTransform t && t.parent is RectTransform pt)
                {
                    // Защита от деления на ноль, если родитель имеет нулевой размер
                    if (pt.rect.width == 0 || pt.rect.height == 0) continue;

                    Undo.RecordObject(t, "Anchors to Center");

                    // 1. Находим геометрический центр нашего объекта в локальных координатах родителя
                    Vector2 localCenter = (Vector2)t.localPosition + t.rect.center;

                    // 2. Переводим этот центр в нормализованные координаты (от 0 до 1)
                    Vector2 normalizedCenter = new Vector2(
                        (localCenter.x - pt.rect.xMin) / pt.rect.width,
                        (localCenter.y - pt.rect.yMin) / pt.rect.height
                    );

                    // 3. Схлопываем якоря ровно в высчитанный центр
                    t.anchorMin = normalizedCenter;
                    t.anchorMax = normalizedCenter;

                    // 4. Жестко фиксируем старые размеры объекта (чтобы он не сжался)
                    t.sizeDelta = new Vector2(t.rect.width, t.rect.height);

                    // 5. САМОЕ ВАЖНОЕ: Компенсируем сдвиг. 
                    // Так как якорь теперь находится ровно в геометрическом центре объекта,
                    // смещение от якоря до Пивота (anchoredPosition) будет равно инвертированному центру.
                    t.anchoredPosition = -t.rect.center;
                }
            }
        }
    }
}