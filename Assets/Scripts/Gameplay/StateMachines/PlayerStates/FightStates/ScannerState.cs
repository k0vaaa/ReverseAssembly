using Core.Input;
using Gameplay.Anims;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.Controllers.Player;
using Reflex.Attributes;

namespace Gameplay.StateMachines.PlayerStates.FightStates
{
    public class ScannerState : FightPlayerState
    {
        [Inject] private InputManager _input;

        public ScannerState(FightController fight, AbilitiesController abilities, IPlayerAnimator animator) :
            base(fight, abilities, animator)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _input.OnInteractPressed += Abilities.TryGetSkill<ScannerSkill>().TryInteract;
        }

        public override void Exit()
        {
            base.Exit();
            _input.OnInteractPressed -= Abilities.TryGetSkill<ScannerSkill>().TryInteract;
        }
    }
}