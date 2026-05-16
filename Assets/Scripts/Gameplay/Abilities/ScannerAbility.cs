using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.ScriptableObjects;
using Gameplay.Controllers.Player;

namespace Gameplay.Abilities
{
    public class ScannerAbility : Skill
    {
        private readonly ScannerController _scannerController;

        public ScannerAbility(SkillData skillData, ScannerController scannerController) : base(skillData)
        {
            _scannerController = scannerController;
        }

        protected override void CastAction()
        {
            _scannerController.ToggleScanner();
        }
    }
}