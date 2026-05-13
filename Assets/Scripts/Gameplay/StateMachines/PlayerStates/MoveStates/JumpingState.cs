using System.Collections;
using Core.Input;
using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.MoveStates
{
    public class JumpingState : MovementPlayerState
    {
        public JumpingState(MovementController movementController, IPlayerAnimator animator) : base(movementController, animator)
        {
        }

        public override void Enter()
        {
            MonoBehaviour.print("Entering Jumping State");
            MovementController.StartCoroutine(JumpRoutine());
        }

        public override void Execute()
        {
            MovementController.CurrentMoveSpeed -= MovementController.WalkSpeed * Time.deltaTime;
            MovementController.Move();
        }

        public override void Exit()
        {   
            MonoBehaviour.print("Exiting Jumping State");
            
        }

        private IEnumerator JumpRoutine()
        {
            PlayerAnimator.DoJump();
            yield return new WaitUntil(() => PlayerAnimator.CheckAnimationState((int)LayerNames.Movement, .63f, "Jump"));
            MovementController.Jump();
            yield return new WaitUntil(() => PlayerAnimator.CheckAnimationState((int)LayerNames.Movement, .99f, "Jump"));
        }
    }
}
