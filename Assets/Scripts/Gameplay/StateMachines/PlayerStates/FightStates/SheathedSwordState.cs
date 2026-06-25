using System.Collections;
using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class SheathedSwordState : FightPlayerState
    {
        public SheathedSwordState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
        }
        
        protected override void EnterAction()
        {
            Debug.Log("Entering Sheathed Sword");
            if (Fight.IsSheathed) return;
            Animator.DoSheath();
            Fight.StartCoroutine(Sheath());
        }

        protected override void ExitAction()
        {
            Debug.Log("Exiting Sheathed Sword");
            //PlayerAnimator.DoWithdraw();
        }
        
        private IEnumerator Sheath()
        {
            yield return new WaitUntil(()=>Animator.CheckAnimationState((int)LayerNames.Fight, 0.8f, "Sheath"));
            Fight.hipSwordGameObject.SetActive(true);
            yield return new WaitUntil(()=>Animator.CheckAnimationState((int)LayerNames.Fight, 0.99f, "Sheath"));
            Fight.IsSheathed = true;
        }
    }
}