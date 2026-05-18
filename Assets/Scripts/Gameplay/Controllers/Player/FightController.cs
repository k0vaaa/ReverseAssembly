using System;
using Core.Input;
using Core.Inventory;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.StateMachines.PlayerStates.FightStates;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class FightController : StateBehaviourController
    {
        private IPlayerAnimator _playerAnimator;
        private AbilitiesController _abilitiesController;
        [Inject] private InputManager _input;

        [SerializeField] private AnimationEventsHandler _crowbarAnimHandler;
        [SerializeField] private AnimationEventsHandler _gunAnimHandler;

        [HideInInspector] public bool IsSheathed;
        [SerializeField] public GameObject hipSwordGameObject;
        [Inject] private Container _container;

        [SerializeField] private GameObject _crowbar;
        [SerializeField] private GameObject _pistol;
        private Skill _currentAttackSkill;


        private void Awake()
        {
            _stateMachine = new StateMachine();
            _abilitiesController = GetComponent<AbilitiesController>();
            _playerAnimator = GetComponent<IPlayerAnimator>();
            GetComponent<IHittable>().onHit.AddListener(_playerAnimator.DoHit);
        }


        private void Start()
        {
            AttackStatesInit();
            EquipCrowbar();
        }

        private void OnEnable()
        {
            BindInput();
            _crowbarAnimHandler.OnAnimationEnded += HandleAttackAnimEnd;
            _gunAnimHandler.OnAnimationEnded += HandleAttackAnimEnd;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void AttackStatesInit()
        {
            //_spellState = new SpellState(this, _skillsController, _playerAnimator);
            //_sheathState = new SheathedSwordState(this, _skillsController, _playerAnimator);

            var meleeState = new MeleeState(this, _abilitiesController, _playerAnimator);
            var rangedState = new RangedSkillState(this, _abilitiesController, _playerAnimator);
            var idleAttackState = new IdleAttackState(this, _abilitiesController, _playerAnimator);
            var scannerState = new ScannerState(this, _abilitiesController, _playerAnimator);

            AttributeInjector.Inject(scannerState, _container);
            AttributeInjector.Inject(meleeState, _container);

            _states[typeof(IdleAttackState)] = idleAttackState;
            _states[typeof(ScannerState)] = scannerState;
            _states[typeof(MeleeState)] = meleeState;
            _states[typeof(RangedSkillState)] = rangedState;
            
            _stateMachine.AddManualTransition(idleAttackState, scannerState);
            _stateMachine.AddManualTransition(idleAttackState, meleeState);
            _stateMachine.AddManualTransition(idleAttackState, rangedState);
            _stateMachine.AddManualTransition(scannerState, idleAttackState);
            _stateMachine.AddManualTransition(meleeState, idleAttackState);
            _stateMachine.AddManualTransition(rangedState, idleAttackState);


            _stateMachine.TrySetState(idleAttackState);
        }

        private void BindInput()
        {
            _input.OnScannerPressed += HandleScannerPress;
            _input.OnLeftClick += HandleAttack;
            _input.OnSlotOnePressed += EquipCrowbar;
            _input.OnSlotTwoPressed += EquipPistol;
        }

        private void UnbindInput()
        {
            _input.OnScannerPressed -= HandleScannerPress;
            _input.OnLeftClick -= HandleAttack;
            _input.OnSlotOnePressed -= EquipCrowbar;
            _input.OnSlotTwoPressed -= EquipPistol;
        }

        private void HandleAttackAnimEnd()
        {
            TryRequestState<IdleAttackState>();
        }

        private void HandleScannerPress()
        {
            _abilitiesController.TryGetSkill<ScannerSkill>().TryCast();
        }

        private void HandleAttack()
        {
            _currentAttackSkill?.TryCast();
        }

        private void EquipCrowbar()
        {
            _crowbar.SetActive(true);
            _pistol.SetActive(false);
            _currentAttackSkill = _abilitiesController.TryGetSkill<MeleeSkill>();
        }

        private void EquipPistol()
        {
            _crowbar.SetActive(false);
            _pistol.SetActive(true);
            _currentAttackSkill = _abilitiesController.TryGetSkill<ProjectileSkill>();
        }


        private void OnDisable()
        {
            UnbindInput();
            _crowbarAnimHandler.OnAnimationEnded -= HandleAttackAnimEnd;
            _gunAnimHandler.OnAnimationEnded -= HandleAttackAnimEnd;
        }
    }
}
