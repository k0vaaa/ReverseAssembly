using Core.Input;
using Core.UI;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Gameplay.Core;
using Gameplay.UI;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class TerminalState : BrainState
    {
        [Inject] private readonly AbilitiesController _abilities;
        [Inject] private readonly InputManager _input;
        [Inject] private BranchManager _branchManager;
        [Inject] private Window _window;
        private  TerminalView _terminalView;

        public TerminalState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
            
        }

        public void Init()
        {
            _terminalView = _window.GetView<TerminalView>();
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = false;
            _input.OnInteractPressed += HandleInteract;
            _terminalView.UpdateInfo(_branchManager.CurrentBranch);
            _terminalView.Show();
        }

        public override void Exit()
        {
           _input.OnInteractPressed -= HandleInteract;
           _terminalView.Hide();
        }

        private void HandleInteract()
        {
            _abilities.TryGetSkill<SwitchBranchSkill>().TryCast();
        }
    }
}