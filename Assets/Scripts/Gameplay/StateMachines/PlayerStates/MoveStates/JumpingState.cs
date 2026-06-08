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

        protected override void EnterAction()
        {
            MonoBehaviour.print("Entering Jumping State");
            //MovementController.StartCoroutine(JumpRoutine());
            MovementController.Jump();
        }

        protected override void ExecuteAction()
        {
            MovementController.CurrentMoveSpeed -= MovementController.WalkSpeed * Time.deltaTime;
            MovementController.Move();
        }

        protected override void ExitAction()
        {   
            MonoBehaviour.print("Exiting Jumping State");
            
        }

        /*private IEnumerator JumpRoutine()
        {
            PlayerAnimator.DoJump();
            yield return new WaitUntil(() => PlayerAnimator.CheckAnimationState((int)LayerNames.Movement, .63f, "Jump"));
            MovementController.Jump();
            yield return new WaitUntil(() => PlayerAnimator.CheckAnimationState((int)LayerNames.Movement, .99f, "Jump"));
        }*/
        
    }
}
