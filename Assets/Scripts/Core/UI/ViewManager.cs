using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] List<View> _viewsList;
        private Dictionary<Type, View> _views = new();

        private void Awake()
        {
            foreach (var view in _viewsList)
            {
                if (!_views.TryAdd(view.GetType(), view))
                {
                    Debug.LogError($"Error adding view {view.GetType()} to dictionary");
                }
            }
        }

        public T GetView<T>() where T : class
        {
            if (_views.TryGetValue(typeof(T), out var view))
            {
                return view as T;
            }
            Debug.LogError($"View of type {typeof(T)} not found");
            return default;
        }

        public void SwitchViews<T, T1>(T from, T1 to) where T : View where T1 : View
        {
            from.Hide();
            to.Show();
        }
    }
}