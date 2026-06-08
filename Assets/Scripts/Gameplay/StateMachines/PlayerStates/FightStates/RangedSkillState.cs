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

        
        protected override void EnterAction()
        {
            _gunAnimator.DoAction();

        }

        protected override void ExecuteAction()
        {
            
        }

        protected override void ExitAction()
        {
            
        }

       
    }
}
