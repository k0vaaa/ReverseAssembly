using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MissingScriptsFinder
    {
        [MenuItem("Tools/Find Missing Scripts In Scene")]
        public static void FindInScene()
        {
            // Находим вообще все объекты на сцене (даже выключенные)
            GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<GameObject> objectsWithMissingScripts = new List<GameObject>();

            foreach (GameObject go in allObjects)
            {
                // Получаем все компоненты объекта
                Component[] components = go.GetComponents<Component>();
            
                for (int i = 0; i < components.Length; i++)
                {
                    // Если скрипт удален, Unity оставляет его в массиве как null
                    if (components[i] == null)
                    {
                        objectsWithMissingScripts.Add(go);
                        Debug.LogWarning($"[Missing Script] найден на объекте: {go.name}", go);
                        break; // Переходим к следующему объекту
                    }
                }
            }

            // Выделяем все найденные объекты в иерархии!
            if (objectsWithMissingScripts.Count > 0)
            {
                Selection.objects = objectsWithMissingScripts.ToArray();
                Debug.Log($"<b>Найдено проблемных объектов: {objectsWithMissingScripts.Count}</b>. Они выделены в окне Hierarchy.");
            }
            else
            {
                Debug.Log("Всё чисто! Пропавших скриптов на сцене нет.");
            }
        }
    }
}