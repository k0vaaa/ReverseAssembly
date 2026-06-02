using Gameplay.Anims;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class MeleeState : FightPlayerState
    {
        private IWeaponAnimator _crowbarAnimator;
        public MeleeState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) : base(fight, abilities, animator)
        {
            _crowbarAnimator = fight.GetComponentInChildren<CrowbarAnimator>(true);
        }
        

        public override void Enter()
        {
            base.Enter();
            
            Animator.DoAttack();
            _crowbarAnimator.DoAction();
            

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            Abilities.TryGetSkill<MeleeSkill>().ClearCollider();
            Debug.Log("Exiting Melee");
        }
    }
}