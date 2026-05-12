using UnityEditor;
using UnityEngine;

namespace Shortcuts
{
    public class FifineShortcuts
    {
        // Атрибут MenuItem создает кнопку в верхнем меню Unity.
        // Символы в конце: % = Ctrl, # = Shift, & = Alt. 
        // То есть шорткат будет: Ctrl + Alt + 1
        [MenuItem("MyTools/Focus Folders/Prefabs %#&1")]
        public static void FocusPrefabsFolder()
        {
            // Укажи путь к нужной тебе папке внутри проекта
            Object folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs");
        
            if (folder != null)
            {
                // Делаем папку активной (как будто кликнули по ней)
                Selection.activeObject = folder;
                EditorUtility.FocusProjectWindow();
                // "Пингуем" её (папка мигнет желтым цветом в окне Project)
                //EditorGUIUtility.PingObject(folder);
            }
            else
            {
                Debug.LogWarning("Папка не найдена!");
            }
        }

        // Можешь добавить сколько угодно таких папок!
        // Шорткат: Ctrl + Shift + Alt + 2
        [MenuItem("MyTools/Focus Folders/Scripts %#&2")]
        public static void FocusScriptsFolder()
        {
            Object folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Scripts");
            if (folder != null)
            {
                Selection.activeObject = folder;
                EditorUtility.FocusProjectWindow();
                //EditorGUIUtility.PingObject(folder);
            }
        }
    }
}