using Gameplay.Anims;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.AbilityStates
{
    public class SwitchBranchAbilityState : FightPlayerState 
    {
        public SwitchBranchAbilityState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SkillsController.Skills[SkillType.BranchSwitch].Cast();
        }
    }
}