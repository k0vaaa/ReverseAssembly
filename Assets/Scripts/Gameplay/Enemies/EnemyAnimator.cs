using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hitted = Animator.StringToHash("Hitted");
        private static readonly int Spell = Animator.StringToHash("Spell");
        
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
        }

        public void IdleEvent()
        {
            _animator.SetBool(Walk, false);
        
        }
        
        public void DoAttack()
        {
            _animator.SetTrigger(Attack);
        }
        
        public void DoHitEvent()
        {
            _animator.SetTrigger(Hitted);
        }

        public void DoSpellEvent()
        {
            _animator.SetTrigger(Spell);
        }
        
        
        
        
        
    }
}