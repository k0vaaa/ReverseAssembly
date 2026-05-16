using System.Collections;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class SpellState : FightPlayerState
    {
        public SpellState(FightController fightController, SkillsController skillsController, IPlayerAnimator animator) : base(fightController, skillsController, animator)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering Spell");
            PlayerAnimator.DoSpell();
            //FightController.swordGameObject.SetActive(false);
            FightController.StartCoroutine(SpellCast());

        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            //FightController.swordGameObject.SetActive(true);
            Debug.Log("Exiting Spell");
        }

        private IEnumerator SpellCast()
        {
            yield return new WaitUntil(()=>PlayerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.425f, "Spell"));
            SkillsController.Skills[SkillType.Fireball].TryCast();
        }
    }
}
