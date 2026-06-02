using System;
using System.Collections.Generic;

namespace Core.StateMachines
{
    public class StateMachine
    {
        private IState _currentState;
        private IState _previousState;

        public Type CurrentState => _currentState.GetType();
        public Type PreviousState => _previousState.GetType();


        private Dictionary<Type, HashSet<Transition>> _states = new Dictionary<Type, HashSet<Transition>>();

        private Dictionary<Type, HashSet<Type>> _antiStates = new();
        private Dictionary<Type, HashSet<Type>> _manualTransitions = new();

        private HashSet<Transition> _currentStates = new HashSet<Transition>();
        private HashSet<Transition> _anyStates = new HashSet<Transition>();

        private HashSet<Transition> _emptyStates = new HashSet<Transition>(0);

        public void Tick()
        {
            var transition = GetTransition();

            if (transition != null)
            {
                ForceSetState(transition.To);
            }

            _currentState?.Execute();
        }

        public void AddManualTransition(IState from, IState to)
        {
            var fromType = from.GetType();
            if (!_manualTransitions.ContainsKey(fromType))
            {
                _manualTransitions[fromType] = new HashSet<Type>();
            }

            _manualTransitions[fromType].Add(to.GetType());
        }

        
        public bool TrySetState(IState nextState)
        {

            if (_currentState == null)
            {
                ForceSetState(nextState);
                return true;
            }

            var currentType = _currentState.GetType();
            var nextType = nextState.GetType();

            bool isAllowed = false;
        
            if (_manualTransitions.TryGetValue(currentType, out var allowedTypes))
            {
                if (allowedTypes.Contains(nextType)) 
                {
                    isAllowed = true;
                }
            }


            if (!isAllowed)
            {
                return false;
            }
            
            ForceSetState(nextState);
            return true;
        }
        
        
        public void ForceSetState(IState nextState)
        {
            if (nextState == _currentState)
            {
                return;
            }

            if (_currentState != null)
            {
                _antiStates.TryGetValue(_currentState.GetType(), out var states);
                if (states != null && states.Contains(nextState.GetType()))
                {
                    return;
                }
            }

            _currentState?.Exit();

            _previousState = _currentState;
            _currentState = nextState;

            _states.TryGetValue(_currentState.GetType(), out _currentStates);

            if (_currentStates == null)
            {
                _currentStates = _emptyStates;
            }

            _currentState?.Enter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (predicate == null)
            {
                return;
            }

            if (_states.TryGetValue(from.GetType(), out var states) == false)
            {
                states = new HashSet<Transition>();
                _states[from.GetType()] = states;
            }

            states.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            if (predicate == null)
            {
                return;
            }

            _anyStates.Add(new Transition(state, predicate));
        }

        public void AddAntiState(IState targetState, IState antiState)
        {
            var type = targetState.GetType();
            var item = antiState.GetType();
            if (!_antiStates.ContainsKey(type))
            {
                var states = new HashSet<Type>();
                _antiStates.Add(type, states);
            }

            _antiStates[type].Add(item);
        }

        private Transition GetTransition()
        {
            foreach (var state in _anyStates)
            {
                if (state.Condition())
                {
                    return state;
                }
            }

            foreach (var state in _currentStates)
            {
                if (state.Condition())
                {
                    return state;
                }
            }

            return null;
        }

        public void ForcePreviousState()
        {
            ForceSetState(_previousState);
        }
    }
}