using System.Collections;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class AttackState : FightPlayerState
    {
        
        public AttackState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }
        

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entering Melee");
            FightController.StartCoroutine(SwordColliderSwitch());
            PlayerAnimator.DoAttack();
            SkillsController.Skills[SkillType.Melee].Cast();

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            Debug.Log("Exiting Melee");
        }

        private IEnumerator SwordColliderSwitch()
        {
            yield return new WaitUntil(()=>PlayerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.4f, "Attack"));
            FightController.SwordCollider.enabled = true;
            yield return new WaitUntil(()=>PlayerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.53f, "Attack"));
            FightController.SwordCollider.enabled = false;
        }
    }
}