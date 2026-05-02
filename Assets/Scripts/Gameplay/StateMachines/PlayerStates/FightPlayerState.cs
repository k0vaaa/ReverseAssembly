using Gameplay.Anims;
using Gameplay.Controllers.Player;

namespace Gameplay.StateMachines.PlayerStates
{
    public abstract class FightPlayerState : IState
    {
        protected FightController FightController;
        protected SkillsController SkillsController;
        protected PlayerAnimator PlayerAnimator;
        
        protected FightPlayerState(FightController fightController, SkillsController skillsController, PlayerAnimator animator)
        {
            FightController = fightController;
            SkillsController = skillsController;
            PlayerAnimator = animator;
        }
        
        public virtual void Enter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
        }
    }
}