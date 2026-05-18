using System.Collections;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class RangedSkillState : FightPlayerState
    {
        private IWeaponAnimator _gunAnimator;
        public RangedSkillState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
            _gunAnimator = fight.GetComponentInChildren<GunAnimator>(true);
        }

        
        public override void Enter()
        {
            Debug.Log("Entering Spell");
            _gunAnimator.DoAction();

        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            Debug.Log("Exiting Spell");
        }

       
    }
}
