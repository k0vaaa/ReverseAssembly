using System;
using System.Collections.Generic;
using Core.Bootstrap;
using Core.Extensions;
using Core.UI.Types;
using UnityEngine;

namespace Core.UI
{
    public class Window : View, IInitializable
    {
        [SerializeField] private SerializableDictionary<View> _views = new();
        private Stack<View> _stack = new();
        private View _stackRoot;

        public void Init()
        {
        }

        public T GetView<T>() where T : View
        {
            if (_views.TryGetValue(out T view))
            {
                return view;
            }

            Debug.LogError($"View of type {view.TypeName()} not found");
            return null;
        }

        public View GetView(Type type)
        {
            if (_views.TryGetValue(type, out var view))
            {
                return view;
            }

            Debug.LogError($"View of type {view.TypeName()} not found");
            return null;
        }

        public bool TryAddView(View view)
        {
            return _views.TryAdd(view);
        }

        public void SwitchViews<T, T1>(T from, T1 to) where T : View where T1 : View
        {
            from.Hide();
            to.Show();
        }

        public void ShowOnly<TView>() where TView : View
        {
            foreach (var view in _views)
            {
                view.Hide();
            }

            GetView<TView>().Show();
        }

        public void Back()
        {
            if (_stack.Count <= 1) return;

            if (_stack.TryPop(out var view))
            {
                view.Hide();
                if (_stack.TryPeek(out var top))
                {
                    top.Show();
                }
            }
        }

        public void Next<TView>() where TView : View
        {
            var view = GetView<TView>();
            view.Show();
            if (_stack.TryPeek(out var top))
            {
                top.Hide();
            }

            _stack.Push(view);
        }

        public void Next(Type type)
        {
            var view = GetView(type);
            view.Show();
            if (_stack.TryPeek(out var top))
            {
                top.Hide();
            }

            _stack.Push(view);
        }

        public void SetStackRoot<T>() where T : View
        {
            _stackRoot = GetView<T>();
            _stack.Push(_stackRoot);
        }
    }
}