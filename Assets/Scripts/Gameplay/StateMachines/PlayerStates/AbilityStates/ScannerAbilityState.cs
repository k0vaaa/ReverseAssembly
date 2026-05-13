using Gameplay.Anims;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates.AbilityStates
{
    public class ScannerAbilityState : FightPlayerState 
    {
        public ScannerAbilityState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SkillsController.Skills[SkillType.Scanner].Cast();
        }
    }
}