
using Core.Input;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Anims
{
    public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
    {
        [Inject] public InputManager InputManager;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Spell = Animator.StringToHash("Spell");
        
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Landed = Animator.StringToHash("Landed");
        private static readonly int Sheath = Animator.StringToHash("Sheath");
        private static readonly int Withdraw = Animator.StringToHash("Withdraw");
        private static readonly int IsRMB = Animator.StringToHash("IsRMB");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Death = Animator.StringToHash("Death");
        
        private static readonly int Hitted = Animator.StringToHash("Hitted");

        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            SetRMB();
            SetBlend();
        }

        
        
        public bool CheckAnimationState(int layerIndex, float time, string stateName) => 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= time && 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        
        
        public void DoAttack()
        {
            _animator.SetTrigger(Attack);
        }
        
        public void DoSheath()
        {
            _animator.SetTrigger(Sheath);
        }
        
        public void DoWithdraw()
        {
            _animator.SetTrigger(Withdraw);
        }

        public void DoSpell()
        {
            _animator.SetTrigger(Spell);
        }
        
        public void DoHit()
        {
            _animator.SetTrigger(Hitted);
        }
        
        public void DoJump()
        {
            _animator.SetTrigger(Jump);
        }
        
        public void DoWalk()
        {   
            _animator.SetBool(Run, false);
            _animator.SetBool(Walk, true);
        }
        
        
        public void DoRun() 
        {
            _animator.SetBool(Walk, false);
            _animator.SetBool(Run, true);
        }
        
        public void DoIdleMove() 
        {
            _animator.SetBool(Walk, false);
            _animator.SetBool(Run, false);
        }

        public void DoFalling()
        {
            _animator.SetBool(Falling, true);
        }

        public void DoLanding()
        {
            _animator.SetBool(Falling, false);
            _animator.SetTrigger(Landed);
        }

        private void SetBlend()
        {
            Vector2 moveInput = InputManager.MoveInput;
            _animator.SetFloat(MoveX, SnapInput(moveInput.x));
            _animator.SetFloat(MoveY, SnapInput(moveInput.y));
        }
        
        private float SnapInput(float value)
        {
            float threshold = 0.5f;

            if (Mathf.Abs(value) > threshold)
            {
                return Mathf.Sign(value); // Возвращает -1 или 1
            }
            return 0; 
        }

        private void SetRMB()
        {
            bool isRMB = InputManager.RMBInput;
            _animator.SetBool(IsRMB, isRMB);
        }
        public void DoDeath()
        {
            _animator.SetTrigger(Death);
        }
        

    }
    public enum LayerNames
    {
        Movement = 1,
        Fight = 2
    }
}