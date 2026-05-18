using System;
using UnityEngine;

namespace Core.Utilities
{
    [Serializable]
    public struct InterfaceReference<T> where T : class
    {
        [SerializeField] private MonoBehaviour _value;

        public T Value
        {
            get => _value as T;
            set => _value = value as MonoBehaviour;
        }
    }
}