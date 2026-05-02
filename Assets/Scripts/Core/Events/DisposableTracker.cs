using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    public class DisposableTracker : MonoBehaviour
    {
        private readonly List<IDisposable> _disposables = new();
        public void Add(IDisposable disposable) => _disposables.Add(disposable);

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}