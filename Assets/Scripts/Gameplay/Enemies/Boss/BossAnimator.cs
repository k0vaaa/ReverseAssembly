using UnityEngine;

namespace Gameplay.Enemies
{
    public class BossAnimator : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hitted = Animator.StringToHash("Hitted");
        private static readonly int SuperAttack = Animator.StringToHash("SuperAttack");
        
        public Animator _animator { get; private set; }
        
        public bool CheckAnimationState(int layerIndex, float time, string stateName) => 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= time && 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void DeathEvent()
        {
            _animator.SetTrigger(Death);
        }
        
        
        public void WalkEvent()
        {
            _animator.SetBool(Walk, true);
            _animator.SetBool(Idle,false);
        }

        public void IdleEvent()
        {
            _animator.SetBool(Walk, false);
            _animator.SetBool(Idle, true);
        }
        
        public void DoAttack()
        {
            _animator.SetTrigger(Attack);
        }
        
        public void DoHitEvent()
        {
            _animator.SetTrigger(Hitted);
        }

        public void DoSuperAttack()
        {
            _animator.SetTrigger(SuperAttack);
        }
    }
}