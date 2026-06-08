using System;
using Core.StateMachines;

namespace Gameplay.Enemies
{
    public abstract class StatesEnemyConst : State
    {
        protected AIController Controller;
        protected EnemyAnimator EnemyAnimator;
        protected EnemyMover Mover;



        protected StatesEnemyConst(AIController controller, EnemyAnimator animator, EnemyMover mover)
        {
            Controller = controller;
            EnemyAnimator = animator;
            Mover = mover;

        }


    }
}