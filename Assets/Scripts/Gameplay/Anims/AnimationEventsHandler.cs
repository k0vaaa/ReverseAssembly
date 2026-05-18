using System;
using UnityEngine;

namespace Gameplay.Anims
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        public event Action OnAnimationEnded;
        public void ColliderOn()
        {
            _collider.enabled = true;
        }
        public void ColliderOff()
        {
            _collider.enabled = false;
        }

        public void InvokeAnimationEnded()
        {
            OnAnimationEnded?.Invoke();
        }
    
    }
}