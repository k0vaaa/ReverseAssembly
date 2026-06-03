using System.Runtime.InteropServices;
using Core.Input;
using Core.UI;
using Gameplay.Anims;
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
        [Inject] private WristTerminalController _terminalController;
        [Inject] private MockPlayerAnimator _animator;
        

        public TerminalState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain, movement, fight)
        {
            
        }

        public void Init()
        {
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = false;

            _terminalController.SetTerminal(true);
            _input.OnInteractPressed += HandleInteract;
            _animator.DoTerminal(true);
        }

        public override void Exit()
        {
           _input.OnInteractPressed -= HandleInteract;
           _terminalController.SetTerminal(false);
           _animator.DoTerminal(false);
        }
        
        

        private void HandleInteract()
        {
            if (!_abilities.TryGetSkill<SwitchBranchSkill>().TryCast()) return;
            HudSwitcher.Instance?.ToggleTheme();
            Brain.ForcePreviousState();
        }
    }
}