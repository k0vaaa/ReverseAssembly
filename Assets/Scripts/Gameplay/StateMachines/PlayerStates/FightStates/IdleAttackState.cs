using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class IdleAttackState : FightPlayerState
    {
        public IdleAttackState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
        }

        protected override void EnterAction()
        {
            
            if(!Fight.IsSheathed) return;
            Animator.DoWithdraw();
            Fight.IsSheathed = false;

        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
            
        }
        

    }
}