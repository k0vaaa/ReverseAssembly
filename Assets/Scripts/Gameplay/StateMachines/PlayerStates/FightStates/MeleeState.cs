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
        

        protected override void EnterAction()
        {
            
            Animator.DoAttack();
            _crowbarAnimator.DoAction();
            

        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
            Abilities.TryGetSkill<MeleeSkill>().ClearCollider();
            Debug.Log("Exiting Melee");
        }
    }
}