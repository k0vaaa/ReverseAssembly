using Core.DI;
using Core.Input;
using Gameplay.Anims;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.StateMachines;
using Gameplay.StateMachines.PlayerStates.FightStates;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class FightController : MonoBehaviour, IInjectable
    {
        private StateMachine _fightStateMachine;
        private PlayerAnimator _playerAnimator;
        private SkillsController _skillsController;
        [Inject] private InputManager _inputManager;

        [HideInInspector] public bool IsSheathed;

        [SerializeField] public GameObject swordGameObject;
        [SerializeField] public GameObject hipSwordGameObject;
        //public Sword Sword { get; private set; }
        public Collider SwordCollider { get; private set; }


        private void Awake()
        {
            
            _fightStateMachine = new StateMachine();
            _skillsController = GetComponent<SkillsController>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            gameObject.GetComponent<IHittable>().onHit.AddListener(_playerAnimator.DoHit);
            SwordCollider = swordGameObject.GetComponent<BoxCollider>();
        }
        

        private void Start()
        {
            AttackStatesInit();
        }

        private void Update()
        {
            _fightStateMachine.Tick();
        }
        
        private void AttackStatesInit()
        {
            var attackState = new AttackState(this,_skillsController, _playerAnimator);
            var spellState = new SpellState(this,_skillsController, _playerAnimator);
            var idleAttackState = new IdleAttackState(this, _skillsController, _playerAnimator);
            var sheathState = new SheathedSwordState(this,_skillsController,_playerAnimator);


            bool MeleeAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight,0.99f,"Attack");
            bool SpellAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight,0.99f,"Spell");
            bool SheathAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight,0.99f,"Sheath");

            
            //_fightStateMachine.AddTransition(idleAttackState,sheathState, () => _inputManager.IsSheathed);
            //_fightStateMachine.AddTransition(sheathState,idleAttackState, () => !_inputManager.IsSheathed);
            
            _fightStateMachine.AddTransition(idleAttackState,attackState, () => _inputManager.MeleeInput && _skillsController.Skills[SkillType.Melee]._isReady);
            _fightStateMachine.AddTransition(attackState,idleAttackState, MeleeAnimationEnded);
            
            //_fightStateMachine.AddTransition(sheathState,spellState, () => _inputManager.RMBInput && _inputManager.SpellInput && _skillsController.Skills[SkillType.Fireball]._isReady);
            _fightStateMachine.AddTransition(spellState,sheathState, SpellAnimationEnded);
            
            _fightStateMachine.SetState(idleAttackState);
            
        }

        
    }
}