using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class SwitchBranchState : FightPlayerState 
    {
        public SwitchBranchState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
        }

        protected override void EnterAction()
        {
            
        }
    }
}