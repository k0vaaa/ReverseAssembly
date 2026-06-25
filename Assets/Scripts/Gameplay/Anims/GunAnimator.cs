using UnityEngine;

namespace Gameplay.Anims
{
    public class GunAnimator : MonoBehaviour,IWeaponAnimator
    {
        private static readonly int Shot = Animator.StringToHash("Shot");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void DoAction()
        {
            _animator.SetTrigger(Shot);
        }
    }
}