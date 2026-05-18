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

        public override void Enter()
        {
            Debug.Log("Entering Focus");
        }

        public override void Execute()
        {
            
        }
        
        public override void Exit()
        {
            Debug.Log("Exiting Focus");
        }
    }
}