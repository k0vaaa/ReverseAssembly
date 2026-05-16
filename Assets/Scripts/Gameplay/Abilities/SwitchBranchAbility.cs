using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Controllers.Player;
using Gameplay.Core;

namespace Gameplay.Abilities
{
    public class SwitchBranchAbility : Skill
    {
        private readonly PlayerTerminalController _playerTerminalController;

        public SwitchBranchAbility(SkillData skillData, PlayerTerminalController playerTerminalController) :
            base(skillData)
        {
            _playerTerminalController = playerTerminalController;
        }

        protected override void CastAction()
        {
            if (_playerTerminalController.TryJump())
            {
                base.TryCast();
            }
        }
    }
}