using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates
{
    public abstract class FightPlayerState : IState
    {
        protected FightController FightController;
        protected SkillsController SkillsController;
        protected IPlayerAnimator PlayerAnimator;
        private float _enterTime;
        private float _duration = .5f;
        public bool IsFinished => Time.time >= _enterTime + _duration;
        
        protected FightPlayerState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator)
        {
            FightController = fightController;
            SkillsController = skillsController;
            PlayerAnimator = animator;
        }
        
        public virtual void Enter()
        {
            _enterTime = Time.time;
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
