using Core.Input;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Reflex.Attributes;
using Reflex.Core;

namespace Gameplay.StateMachines.PlayerStates.PlayerBrainStates
{
    public class TerminalState : BrainState
    {
        [Inject] private readonly AbilitiesController _abilities;
        
        [Inject] private WristTerminalController _terminalController;
        [Inject] private MockPlayerAnimator _animator;
        [Inject] private Container _container;
        [Inject] private readonly InputManager _input;
        public readonly StateMachine StateMachine;


        public TerminalState(PlayerBrain brain, MovementController movement, FightController fight) : base(brain,
            movement, fight)
        {
            StateMachine = new StateMachine();
        }

        public void Init()
        {
            var branchesState = new BranchesState(this);
            var scannerState = new ScannerState(this);
            var closedState = new ClosedState(this);
            
            StateMachine.AddState(branchesState);
            StateMachine.AddState(scannerState);
            StateMachine.AddState(closedState);
            
            StateMachineInjector.InjectStates(StateMachine, _container);



            StateMachine.AddManualTransition(branchesState,scannerState);
            StateMachine.AddManualTransition(branchesState,closedState);
            StateMachine.AddManualTransition(scannerState,branchesState);
            StateMachine.AddManualTransition(scannerState,closedState);
            StateMachine.AddManualTransition(closedState,branchesState);
            StateMachine.AddManualTransition(closedState,scannerState);

            StateMachine.TrySetState(closedState);
        }

        public override void Enter()
        {
            Movement.enabled = true;
            Fight.enabled = false;
            StateMachine.TryRequestState<BranchesState>();
            //_terminalController.SetTerminal(true);
            
            _input.OnScannerPressed += HandleScannerPress;
            _animator.DoTerminal(true);
        }


        public override void Exit()
        {
            _input.OnScannerPressed -= HandleScannerPress;
            //_terminalController.SetTerminal(false);
            _animator.DoTerminal(false);
            StateMachine.ForceRequestState<ClosedState>();
        }

        private void HandleScannerPress()
        {
            if (_abilities.TryGetSkill<ScannerSkill>().TryCast())
            {
                StateMachine.RequestToggleState<ScannerState, BranchesState>();
            }
        }


        
    }
}