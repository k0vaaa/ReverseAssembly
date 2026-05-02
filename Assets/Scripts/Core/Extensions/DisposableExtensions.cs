using System;
using Core.Events;
using UnityEngine;

namespace Core.Extensions
{
    public static class DisposableExtensions
    {
        public static void AddTo(this IDisposable disposable, GameObject gameObject)
        {
            var tracker = gameObject.GetComponent<DisposableTracker>() ?? gameObject.AddComponent<DisposableTracker>();
            tracker.Add(disposable);
        }
    }
}