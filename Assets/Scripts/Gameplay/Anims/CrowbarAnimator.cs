using UnityEngine;

namespace Gameplay.Anims
{
    public class CrowbarAnimator : MonoBehaviour, IWeaponAnimator
    {
        private static readonly int Swing = Animator.StringToHash("Swing");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void DoAction()
        {
            _animator.SetTrigger(Swing);
        }
    }
}