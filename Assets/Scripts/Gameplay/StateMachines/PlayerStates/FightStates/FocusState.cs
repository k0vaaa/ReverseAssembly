using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class FocusState : FightPlayerState
    {
        public FocusState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
        }

        protected override void EnterAction()
        {
            Debug.Log("Entering Focus");
        }

        protected override void ExecuteAction()
        {
            
        }
        
        protected override void ExitAction()
        {
            Debug.Log("Exiting Focus");
        }
    }
}