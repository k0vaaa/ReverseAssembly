using Core.Extensions;
using Core.UI.Types;
using UnityEngine;

namespace Core.UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<Window> _windows;
        
        public T GetWindow<T>() where T : Window
        {
            if (_windows.TryGetValue(out T window))
            {
                return window;
            }

            Debug.LogError($"Window of type {window.TypeName()} not found");
            return null;
        }

        public bool TryAdd(Window window)
        {
            return _windows.TryAdd(window);
        }

        public void SwitchWindows<T, T1>(T from, T1 to) where T : Window where T1 : Window
        {
            from.Hide();
            to.Show();
        }

        public void ShowOnly<TWindow>() where TWindow : Window
        {
            foreach (var window in _windows)
            {
                window.Hide();
            }

            GetWindow<TWindow>()?.Show();
        }

        public void Show<TWindow>() where TWindow : Window
        {
            GetWindow<TWindow>()?.Show();
        }
        
        public void Hide<TWindow>() where TWindow : Window
        {
            GetWindow<TWindow>()?.Hide();
        }
    }
}