using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Core;
using Gameplay.UI;
using Gameplay.UI.Windows;
using Reflex.Attributes;

namespace Gameplay.Combat.Offensive.Skills.Abilities
{
    public class SwitchBranchSkill : Skill
    {
        [Inject] private BranchManager _branchManager;
        [Inject] private SyncEnergyManager _energyManager;
        private readonly SkillContext _ctx;

        public SwitchBranchSkill(AbilityDefinition abilityDefinition, SkillContext ctx) : base(abilityDefinition)
        {
            _ctx = ctx;
        }

        public override void Init()
        {
        }


        protected override void OnTick()
        {
        }

        protected override bool CastAction()
        {
            if (_energyManager.TryConsumeEnergy())
            {
                _branchManager.ToggleBranch();
                return true;
            }

            return false;

        }

        
    }
}