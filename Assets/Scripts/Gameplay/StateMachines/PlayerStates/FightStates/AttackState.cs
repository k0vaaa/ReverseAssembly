using System.Collections;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class AttackState : FightPlayerState
    {
        private float _enterTime;
        private float _duration = .5f;
        public bool IsFinished => Time.time >= _enterTime + _duration;

        public AttackState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }
        

        public override void Enter()
        {
            Debug.Log("Entering Melee");
            FightController.StartCoroutine(SwordColliderSwitch());
            PlayerAnimator.DoAttack();
            _enterTime = Time.time;
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