using Core.Bootstrap;
using Core.Events;
using Core.Input;
using Core.StateMachines;
using Gameplay.Events;
using Gameplay.StateMachines;
using Gameplay.StateMachines.PlayerStates.PlayerBrainStates;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;
using Logger = Core.Utilities.Logger;

namespace Gameplay.Controllers.Player
{
    public class PlayerBrain : StateBehaviourController, IInitializable
    {
        [Inject] private InputManager _input;
        private MovementController _movement;
        private FightController _fight;
        private AbilitiesController _abilities;
        [Inject] private Container _container;
        

        public void Init()
        {
            StateMachine = new StateMachine();
            _movement = GetComponent<MovementController>();
            _fight = GetComponent<FightController>();
            _abilities = GetComponent<AbilitiesController>();
            
            InitStates();

            _input.OnEscapePressed += HandlePause;
            _input.OnTerminalPressed += HandleTerminal;
            Logger.Trace();
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        private void InitStates()
        {
            var defaultState = new DefaultState(this,_movement, _fight);
            var terminalState = new TerminalState(this,_movement, _fight);
            var pauseState =  new PauseState(this,_movement, _fight);
            var endGameState = new EndGameState(this,_movement, _fight);
            
            StateMachine.AddState(defaultState);
            StateMachine.AddState(terminalState);
            StateMachine.AddState(pauseState);
            StateMachine.AddState(endGameState);
            
            StateMachineInjector.InjectStates(StateMachine, _container);
            
            terminalState.Init();


            


            StateMachine.AddManualTransition(defaultState, terminalState);
            StateMachine.AddManualTransition(terminalState, defaultState); 
            StateMachine.AddManualTransition(pauseState, defaultState);

            EventBus.Subscribe<GameEndedEvent>(HandleGameEnd);
            
            StateMachine.TrySetState(defaultState);
            Debug.Log($"{GetType().Name} States Initialized");
            
        }


        private void HandleTerminal()
        {
            StateMachine.ForceToggleState<TerminalState,DefaultState>();
        }
        private void HandlePause()
        {
            if(StateMachine.CurrentState != typeof(PauseState)) StateMachine.ForceRequestState<PauseState>();
            else StateMachine.ForcePreviousState();
            Logger.Trace();
        }

        private void HandleGameEnd(GameEndedEvent e)
        {
            StateMachine.ForceRequestState<EndGameState>();
        }

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.OnEscapePressed -= HandlePause;
                _input.OnTerminalPressed -= HandleTerminal;
            }

            //StateMachine.ForceRequestState<DefaultState>();
        }
    }
}