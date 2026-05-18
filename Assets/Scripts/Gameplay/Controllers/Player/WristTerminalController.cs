using System;
using Core.UI;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Core;
using Gameplay.StateMachines.PlayerStates.PlayerBrainStates;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;
using InputManager = Core.Input.InputManager;

namespace Gameplay.Controllers.Player
{
    public class WristTerminalController : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;
        [Inject] private SyncEnergyManager _syncEnergyManager;
        [Inject] private PlayerBrain _brain;


        public bool IsTerminalOpen { get; private set; }
        

        private void OnEnable()
        {
            _inputManager.OnTerminalPressed += ToggleTerminal;
        }

        private void OnDisable()
        {
            _inputManager.OnTerminalPressed -= ToggleTerminal;
        }


        public void ToggleTerminal()
        {
            IsTerminalOpen = !IsTerminalOpen;

            if (IsTerminalOpen)
            {
                _brain.TryRequestState<TerminalState>();
            }
            else
            {
                _brain.TryRequestState<DefaultState>();
            }
        }
    }
}