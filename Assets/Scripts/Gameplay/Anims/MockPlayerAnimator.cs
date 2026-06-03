using System;
using UnityEngine;

namespace Gameplay.Anims
{
    public class MockPlayerAnimator : MonoBehaviour, IPlayerAnimator
    {
        private static readonly int Terminal = Animator.StringToHash("Terminal");
        [SerializeField] private Animator _animator;
        public bool CheckAnimationState(int layerIndex, float time, string stateName)
        {
            Debug.LogWarning($"Mock: Checking animation state {stateName} on layer {layerIndex}");
            return false;
        }



        public void DoTerminal(bool on)
        {
            _animator.SetBool(Terminal, on);
        }

        public void DoAttack()
        {
            Debug.LogWarning("Mock: Execute Attack sequence");
        }

        public void DoSheath()
        {
            Debug.LogWarning("Mock: Execute Sheath sequence");
        }

        public void DoWithdraw()
        {
            Debug.LogWarning("Mock: Execute Withdraw sequence");
        }

        public void DoSpell()
        {
            Debug.LogWarning("Mock: Execute Spell/Program cast");
        }

        public void DoHit()
        {
            Debug.LogWarning("Mock: Execute Hit reaction");
        }

        public void DoJump()
        {
            Debug.LogWarning("Mock: Execute Jump animation");
        }

        public void DoWalk()
        {
            Debug.LogWarning("Mock: Execute Walk movement");
        }

        public void DoRun()
        {
            Debug.LogWarning("Mock: Execute Run movement");
        }

        public void DoIdleMove()
        {
            Debug.LogWarning("Mock: Execute Idle movement");
        }

        public void DoFalling()
        {
            Debug.LogWarning("Mock: Execute Falling state");
        }

        public void DoLanding()
        {
            Debug.LogWarning("Mock: Execute Landing state");
        }

        public void DoDeath()
        {
            Debug.LogWarning("Mock: Execute Death/Stability depletion sequence");
        }
    }
}
