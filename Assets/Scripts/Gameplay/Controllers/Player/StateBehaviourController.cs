using System;
using System.Collections.Generic;
using Core.StateMachines;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class StateBehaviourController : MonoBehaviour
    {
        protected StateMachine _stateMachine;
        protected Dictionary<Type, IState> _states = new();
        
        public bool TryRequestState<T>() where T : IState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                return _stateMachine.TrySetState(state);
            }
        
            Debug.LogError($"Стейт {typeof(T).Name} не найден в {GetType().Name}!");
            return false;
        }
        public void ForceRequestState<T>() where T : IState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            { 
                _stateMachine.ForceSetState(state);
                
            }
        
            Debug.LogError($"Стейт {typeof(T).Name} не найден в {GetType().Name}!");
        }
    }
}