using Core.Bootstrap;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Walk = Animator.StringToHash("IsWalking");
        private static readonly int Attack = Animator.StringToHash("Punch");
        private static readonly int Hitted = Animator.StringToHash("Hit");
        //private static readonly int Spell = Animator.StringToHash("Spell");
        
        public Animator _animator { get; private set; }
        
        public bool CheckAnimationState(int layerIndex, float time, string stateName) => 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= time && 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);

        public void Init()
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

        /*public void DoSpellEvent()
        {
            _animator.SetTrigger(Spell);
        }*/
        
        
        
        
        
    }
}