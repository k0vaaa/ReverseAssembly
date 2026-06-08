using System;
using Core.StateMachines;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public abstract class StatesBossConst : State
    {
        protected AIController Controller;
        protected BossAnimator BossAnimator;
        protected EnemyMover Mover;
        private Action _onEnter;
        private Action _onExecute;
        private Action _onExit;

        protected StatesBossConst(AIController controller, BossAnimator animator, EnemyMover mover)
        {
            Controller = controller;
            BossAnimator = animator;
            Mover = mover;
        }
        
        protected override void EnterAction()
        {
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
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