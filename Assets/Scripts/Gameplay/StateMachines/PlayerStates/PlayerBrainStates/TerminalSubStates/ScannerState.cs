using Core.Input;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class ScannerState : TerminalSubState
    {
        [Inject] private InputManager _input;
        [Inject] private AbilitiesController _abilities;
        [Inject] private TerminalWindow _terminalWindow;
        [Inject] private VFXController _vfx;
        

        public ScannerState(TerminalState terminalState) : base(terminalState)
        {
        }

        protected override void EnterAction()
        {
            _terminalWindow.ShowOnly<TerminalScannerView>();
            _input.OnInteractPressed += _abilities.TryGetSkill<ScannerSkill>().TryInteract;
        }

        protected override void ExitAction()
        {
            _input.OnInteractPressed -= _abilities.TryGetSkill<ScannerSkill>().TryInteract;
            _abilities.TryGetSkill<ScannerSkill>().SetScanner(false);
            _vfx.SetProjection(null);
        }
    }
}