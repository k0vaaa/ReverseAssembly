using System;
using Core.StateMachines;

namespace Gameplay.Enemies
{
    public abstract class EnemyState : State
    {
        protected AIController Controller;
        protected EnemyAnimator EnemyAnimator;
        protected EnemyMover Mover;



        protected EnemyState(AIController controller, EnemyAnimator animator, EnemyMover mover)
        {
            Controller = controller;
            EnemyAnimator = animator;
            Mover = mover;

        }


    }
}