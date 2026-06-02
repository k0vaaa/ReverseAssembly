using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    public class FifineShortcuts
    {
        // Атрибут MenuItem создает кнопку в верхнем меню Unity.
        // Символы в конце: % = Ctrl, # = Shift, & = Alt. 
        // То есть шорткат будет: Ctrl + Alt + 1
        [MenuItem("MyTools/Focus Folders/Prefabs %#&1")]
        public static async void FocusPrefabsFolder()
        {
            await FocusOnFolder("Assets/Prefabs");
        }

        // Шорткат: Ctrl + Shift + Alt + 2
        [MenuItem("MyTools/Focus Folders/Scripts %#&2")]
        public static async void FocusScriptsFolder()
        {
            await FocusOnFolder("Assets/Scripts");
        }

        [MenuItem("MyTools/Toggle Inspector Lock %#&3")]
        public static void ToggleInspectorLock()
        {
            // Меняем статус блокировки Инспектора на противоположный
            bool isLocked = ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.isLocked = !isLocked;

            // Принудительно обновляем Инспектор, чтобы иконка замочка визуально изменилась
            ActiveEditorTracker.sharedTracker.ForceRebuild();

            // Приятный бонус: выводим статус в консоль, чтобы точно знать, что кнопка сработала
            if (ActiveEditorTracker.sharedTracker.isLocked)
            {
                Debug.Log("<b>[Fifine]</b> Инспектор <color=red>ЗАБЛОКИРОВАН 🔒</color>");
            }
            else
            {
                Debug.Log("<b>[Fifine]</b> Инспектор <color=green>РАЗБЛОКИРОВАН 🔓</color>");
            }
        }

        private static async Task FocusOnFolder(string path)
        {
            Object folder = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (folder != null)
            {
                GameObject hierarchySelection = null;
        
                if (Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject))
                {
                    hierarchySelection = Selection.activeGameObject;
                }

                Selection.activeObject = folder;
                EditorUtility.FocusProjectWindow();
        
                await Task.Delay(100);
        
                if (hierarchySelection != null)
                {
                    Selection.activeObject = hierarchySelection;
                }
            }
            else
            {
                Debug.LogWarning("Папка не найдена!");
            }
        }
        
        private static void OpenAndFocusFolder(string path)
        {
            Object folder = AssetDatabase.LoadAssetAtPath<Object>(path);
        
            if (folder != null)
            {
                // 1. Делаем окно Project АКТИВНЫМ (переводим на него фокус)
                EditorUtility.FocusProjectWindow();

                // 2. Выделяем папку и подсвечиваем её (Ping)
                Selection.activeObject = folder;
                EditorGUIUtility.PingObject(folder);

                // 3. Заходим внутрь папки (эмуляция двойного клика)
                EnterFolder(folder);
            }
            else
            {
                Debug.LogWarning($"[Fifine] Папка по пути {path} не найдена!");
            }
        }

        // Та самая "чёрная магия" через рефлексию
        private static void EnterFolder(Object folder)
        {
            // Ищем внутренний (скрытый) класс ProjectBrowser
            System.Type projectBrowserType = System.Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
            if (projectBrowserType != null)
            {
                // Получаем текущее открытое окно Project
                EditorWindow window = EditorWindow.GetWindow(projectBrowserType);
                if (window != null)
                {
                    // Ищем скрытый метод "ShowFolderContents", который отвечает за вход в папку
                    MethodInfo showFolderMethod = projectBrowserType.GetMethod(
                        "ShowFolderContents",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    if (showFolderMethod != null)
                    {
                        // Вызываем этот метод, передавая ему ID нашей папки
                        showFolderMethod.Invoke(window, new object[] { folder.GetInstanceID(), true });
                    }
                }
            }
        }

    }
}