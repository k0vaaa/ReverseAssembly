using System;
using System.Collections.Generic;
using Core.Bootstrap;
using UnityEngine;

namespace Core.UI
{
    public class ViewManager : MonoBehaviour, IInitializable
    {
        [SerializeField] List<View> _viewsList;
        private Dictionary<Type, View> _views = new();

        public void Init()
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
            return null;
        }

        public void SwitchViews<T, T1>(T from, T1 to) where T : View where T1 : View
        {
            from.Hide();
            to.Show();
        }
    }
}