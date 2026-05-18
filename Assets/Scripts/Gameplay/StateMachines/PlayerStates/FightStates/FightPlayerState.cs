using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public abstract class FightPlayerState : IState
    {
        protected FightController Fight;
        protected AbilitiesController Abilities;
        protected IPlayerAnimator Animator;
        
        
        protected FightPlayerState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator)
        {
            Fight = fight;
            Abilities = abilities;
            Animator = animator;
        }
        
        public virtual void Enter()
        {
            Debug.Log($"Entering {GetType().Name}");
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
            Debug.Log($"Exiting {GetType().Name}");
        }
    }
}
