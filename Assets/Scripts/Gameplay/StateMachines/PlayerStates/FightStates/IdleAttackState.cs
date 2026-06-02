using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class IdleAttackState : FightPlayerState
    {
        public IdleAttackState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if(!Fight.IsSheathed) return;
            Animator.DoWithdraw();
            Fight.IsSheathed = false;

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            base.Exit();
        }
        

    }
}