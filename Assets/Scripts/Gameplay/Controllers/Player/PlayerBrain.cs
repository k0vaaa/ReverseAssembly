using System;
using Core.Input;
using Core.StateMachines;
using Gameplay.StateMachines.PlayerStates.PlayerBrainStates;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using Logger = Core.Utilities.Logger;

namespace Gameplay.Controllers.Player
{
    public class PlayerBrain : StateBehaviourController
    {
        [Inject] private InputManager _input;
        private MovementController _movement;
        private FightController _fight;
        private AbilitiesController _abilities;
        [Inject] private Container _container;
        

        public void Init()
        {
            _stateMachine = new StateMachine();
            _movement = GetComponent<MovementController>();
            _fight = GetComponent<FightController>();
            _abilities = GetComponent<AbilitiesController>();
            
            
            InitStates();
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void InitStates()
        {
            var defaultState = new DefaultState(this,_movement, _fight);
            var terminalState = new TerminalState(this,_movement, _fight);
            var pauseState =  new PauseState(this,_movement, _fight);
            AttributeInjector.Inject(pauseState, _container);
            AttributeInjector.Inject(defaultState, _container);
            AttributeInjector.Inject(terminalState, _container);
            terminalState.Init();
            _states[typeof(DefaultState)] = defaultState;
            _states[typeof(TerminalState)] = terminalState;
            _states[typeof(PauseState)] = pauseState;

            _stateMachine.AddManualTransition(defaultState, terminalState);
            _stateMachine.AddManualTransition(terminalState, defaultState); 
            _stateMachine.AddManualTransition(pauseState, defaultState); 
            
            _stateMachine.TrySetState(defaultState);
            Debug.Log($"{GetType().Name} States Initialized");
            
        }

        private void OnEnable()
        {
            _input.OnEscapePressed += HandlePause;
            Logger.Trace();
            
        }
        private void OnDisable()
        {
            _input.OnEscapePressed -= HandlePause;
            Logger.Trace();
            
        }


        private void HandlePause()
        {
            if(_stateMachine.CurrentState != typeof(PauseState)) ForceRequestState<PauseState>();
            else ForcePreviousState();
            Logger.Trace();
        }

        private void OnDestroy()
        {
            ForceRequestState<DefaultState>();
        }
    }
}