using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI.Types
{
    [Serializable]
    public class SerializableDictionary<T> : IEnumerable<T>, ISerializationCallbackReceiver where T : class
    {
        [SerializeField] private List<T> _values = new();
        // ctor
        public void OnAfterDeserialize()
        {
            _tDictionary.Clear();
            foreach (var value in _values)
            {
                if (value == null) continue;
                
                if (!_tDictionary.TryAdd(value.GetType(), value))
                {
                    Debug.LogError($"Error adding value {value.GetType()} to dictionary");
                }
            }
        }

        private Dictionary<Type, T> _tDictionary = new();

        public TValue GetValue<TValue>() where TValue : class, T
        {
            if (_tDictionary.TryGetValue(typeof(TValue), out var value))
            {
                return value as TValue;
            }

            Debug.LogError($"Value of type {typeof(TValue)} not found");
            return null;
        }

        public bool TryGetValue<TValue>(out TValue value) where TValue : class, T
        {
            if (_tDictionary.TryGetValue(typeof(TValue), out var v))
            {
                value = v as TValue;
                return true;
            }

            Debug.LogError($"Value of type {typeof(TValue).Name} not found");
            value = null;
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
        }
    }
}