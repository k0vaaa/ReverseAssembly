using System;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public abstract class FightPlayerState : State
    {
        protected FightController Fight;
        protected AbilitiesController Abilities;
        protected IPlayerAnimator Animator;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;


        protected FightPlayerState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator)
        {
            Fight = fight;
            Abilities = abilities;
            Animator = animator;
        }
        
        protected override void EnterAction()
        {
            Debug.Log($"Entering {GetType().Name}");
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
            Debug.Log($"Exiting {GetType().Name}");
        }

        public Action OnEnter
        {
            get => _onEnter;
            set => _onEnter = value;
        }

        public Action OnExecute
        {
            get => _onExecute;
            set => _onExecute = value;
        }

        public Action OnExit
        {
            get => _onExit;
            set => _onExit = value;
        }
    }
}
