using Core.Input;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.Core;
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class BranchesState : TerminalSubState
    {
        [Inject] private AbilitiesController _abilities;
        [Inject] private readonly InputManager _input;
        [Inject] private TerminalWindow _terminalWindow;
        [Inject] private PlayerBrain _brain;
        public BranchesState(TerminalState terminalState) : base(terminalState)
        {
        }

        protected override void EnterAction()
        {
            _terminalWindow.ShowOnly<TerminalView>();
            _terminalWindow.GetView<TerminalView>().UpdateInfo(BranchManager.CurrentBranch);
            _input.OnInteractPressed += HandleInteract;
        }
        
        protected override void ExitAction()
        {
            _input.OnInteractPressed -= HandleInteract;
            _terminalWindow.GetView<TerminalView>().UpdateInfo(BranchManager.CurrentBranch);
        }

        private void HandleInteract()
        {
            if (!_abilities.TryGetSkill<SwitchBranchSkill>().TryCast()) return;
            HudSwitcher.Instance?.ToggleTheme();
            _brain.StateMachine.ForceRequestState<DefaultState>();
        }
    }
}