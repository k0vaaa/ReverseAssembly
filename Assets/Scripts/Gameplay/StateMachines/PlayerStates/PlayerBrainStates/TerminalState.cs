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
        private PlayerHUDView _hudView;
        public TerminalState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
            
        }

        public void Init()
        {
            _terminalView = _window.GetView<TerminalView>();
            _hudView = _window.GetView<PlayerHUDView>();
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = false;
            _input.OnInteractPressed += HandleInteract;
            _terminalView.UpdateInfo(_branchManager.CurrentBranch);
            _hudView.Hide();
            _terminalView.Show();
                
        }

        public override void Exit()
        {
           _input.OnInteractPressed -= HandleInteract;
           _hudView.Show();
           _terminalView.Hide();
        }

        private void HandleInteract()
        {
            _abilities.TryGetSkill<SwitchBranchSkill>().TryCast();
        }
    }
}