using System.Collections;
using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class IdleAttackState : FightPlayerState
    {
        public IdleAttackState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering IdleAttack");
            if(!FightController.IsSheathed) return;
            PlayerAnimator.DoWithdraw();
            FightController.IsSheathed = false;
            FightController.StartCoroutine(Withdraw());
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            Debug.Log("Exiting IdleAttack");
        }
        
        private IEnumerator Withdraw()
        {
            yield return new WaitUntil(()=>PlayerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.374f, "Withdraw"));
            FightController.swordGameObject.SetActive(true);
            FightController.hipSwordGameObject.SetActive(false);
        }
    }
}