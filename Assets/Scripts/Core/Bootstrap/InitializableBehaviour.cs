using UnityEngine;

namespace Core.Bootstrap
{
    public abstract class InitializableBehaviour : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }

        public void Init()
        {
            if (IsInitialized) return;
            OnInit();
            IsInitialized = true;
        }

        protected abstract void OnInit();
    }
}