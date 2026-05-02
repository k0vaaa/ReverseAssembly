using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Observers
{
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerExitEvent;

        private HashSet<Collider> _others = new();

        private void OnTriggerEnter(Collider other)
        {
            HandleEnter(other);
        }

        private void HandleEnter(Collider other)
        {
            if (_others.Add(other))
            {
                OnTriggerEnterEvent?.Invoke(other);
            }
        }

        private void FixedUpdate()
        {
            var disabled = _others.Where(x => !x.gameObject.activeInHierarchy).ToList();
            foreach (var item in disabled)
            {
                HandleExit(item);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            HandleExit(other);
        }

        private void HandleExit(Collider other)
        {
            if (_others.Remove(other))
            {
                OnTriggerExitEvent?.Invoke(other);
            }
        }
    }
}