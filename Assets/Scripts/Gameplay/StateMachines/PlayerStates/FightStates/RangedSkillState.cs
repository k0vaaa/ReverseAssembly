using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class RangedSkillState : FightPlayerState
    {
        private IWeaponAnimator _gunAnimator;
        public RangedSkillState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
            _gunAnimator = fight.GetComponentInChildren<GunAnimator>(true);
        }

        
        public override void Enter()
        {
            base.Enter();
            _gunAnimator.DoAction();

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
