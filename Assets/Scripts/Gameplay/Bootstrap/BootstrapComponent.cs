using System;
using Core.Extensions;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public abstract class BootstrapComponent : MonoBehaviour
    {
        public event Action OnBooted;
        public void Boot()
        {
            Debug.Log($"{this.TypeName()} started booting");
            OnBoot();
            Debug.Log($"{this.TypeName()} booted");
            OnBooted?.Invoke();
        }
        protected virtual void OnBoot(){}
    }
}