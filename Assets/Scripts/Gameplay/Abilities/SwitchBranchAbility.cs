using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Core;

namespace Gameplay.Abilities
{
    public class SwitchBranchAbility : Skill
    {
        private readonly BranchManager _branchManager;

        public SwitchBranchAbility(SkillData skillData, BranchManager branchManager) : base(skillData)
        {
            _branchManager = branchManager;
        }

        public override void Cast()
        {
            base.Cast();
            _branchManager.ToggleBranch();
        }
    }
}