using UnityEngine;
using UnityEditor;

public class SmartCreateEmpty
{
    [MenuItem("GameObject/Smart Create Empty %#&n", false, 0)]
    public static void CreateContextEmpty()
    {
        GameObject parent = Selection.activeGameObject;
        GameObject newObject;

        // 1. Проверяем, есть ли выделенный объект и является ли он элементом UI
        if (parent != null && parent.GetComponent<RectTransform>() != null)
        {
            // Создаем объект и сразу добавляем ему RectTransform вместо обычного Transform
            newObject = new GameObject("GameObject", typeof(RectTransform));
        }
        else
        {
            // Создаем стандартный 3D-объект
            newObject = new GameObject("GameObject");
        }

        // 2. Если родитель есть, удочеряем и выравниваем по центру
        if (parent != null)
        {
            GameObjectUtility.SetParentAndAlign(newObject, parent);
        }

        // 3. Регистрируем для системы отмены (Ctrl+Z)
        Undo.RegisterCreatedObjectUndo(newObject, "Smart Create Empty");

        // 4. Переводим фокус на новый объект
        Selection.activeObject = newObject;
    }
}