using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Core;
using Gameplay.UI;
using Reflex.Attributes;

namespace Gameplay.Combat.Offensive.Skills.Abilities
{
    public class SwitchBranchSkill : Skill
    {
        [Inject] private BranchManager _branchManager;
        private readonly SkillContext _ctx;

        public SwitchBranchSkill(AbilityDefinition abilityDefinition, SkillContext ctx) : base(abilityDefinition)
        {
            _ctx = ctx;
        }
        

        protected override void OnTick()
        {
        }

        protected override bool CastAction()
        {
            _branchManager.ToggleBranch();
            HudSwitcher.Instance?.ToggleTheme();
            return true;
        }

        
    }
}