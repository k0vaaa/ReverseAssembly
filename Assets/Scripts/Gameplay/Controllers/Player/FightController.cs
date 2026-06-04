using System;
using Core.Bootstrap;
using Core.Extensions;
using Core.Input;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Combat.Offensive.Skills.Abilities;
using Gameplay.StateMachines.PlayerStates.FightStates;
using Gameplay.StateMachines.PlayerStates.PlayerBrainStates;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class FightController : StateBehaviourController, IInitializable
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
        private bool _isInitialized;


        public void Init()
        {
            GetComponents();
            AttackStatesInit();
            EquipCrowbar();
            
            _crowbarAnimHandler.OnAnimationEnded += HandleAttackAnimEnd;
            _gunAnimHandler.OnAnimationEnded += HandleAttackAnimEnd;
            _isInitialized = true;
            enabled = true;
        }

        private void GetComponents()
        {
            StateMachine = new StateMachine();
            _abilitiesController = GetComponent<AbilitiesController>();
            _playerAnimator = GetComponent<IPlayerAnimator>();
            GetComponent<IHittable>().onHit.AddListener(_playerAnimator.DoHit);
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        private void AttackStatesInit()
        {
            var meleeState = new MeleeState(this, _abilitiesController, _playerAnimator);
            var rangedState = new RangedSkillState(this, _abilitiesController, _playerAnimator);
            var idleAttackState = new IdleAttackState(this, _abilitiesController, _playerAnimator);
            

            AttributeInjector.Inject(meleeState, _container);

            StateMachine.AddState(idleAttackState);
            StateMachine.AddState(meleeState);
            StateMachine.AddState(rangedState);
            
            StateMachine.AddManualTransition(idleAttackState, meleeState);
            StateMachine.AddManualTransition(idleAttackState, rangedState);

            StateMachine.AddManualTransition(meleeState, idleAttackState);
            StateMachine.AddManualTransition(rangedState, idleAttackState);


            StateMachine.TrySetState(idleAttackState);
        }

        private void OnEnable()
        {
            BindInput();
        }
        
        private void OnDisable()
        {
            UnbindInput();
        }


        private void BindInput()
        {
            
            _input.OnLeftClick += HandleAttack;
            _input.OnSlotOnePressed += EquipCrowbar;
            _input.OnSlotTwoPressed += EquipPistol;
        }

        private void UnbindInput()
        {
            
            _input.OnLeftClick -= HandleAttack;
            _input.OnSlotOnePressed -= EquipCrowbar;
            _input.OnSlotTwoPressed -= EquipPistol;
        }

        private void HandleAttackAnimEnd()
        {
            StateMachine.TryRequestState<IdleAttackState>();
        }

        

        private void HandleAttack()
        {
            _currentAttackSkill?.TryCast();
        }

        private void EquipCrowbar()
        {
            if (!_abilitiesController.TryGetSkill<ScannerSkill>().IsScannerActive)
            {
                StateMachine.TryRequestState<IdleAttackState>();
            }
            _crowbar.transform.GetChild(0).LocalResetAll();
            _crowbar.SetActive(true);
            _pistol.SetActive(false);
            _currentAttackSkill = _abilitiesController.TryGetSkill<MeleeSkill>();
        }

        private void EquipPistol()
        {
            if (!_abilitiesController.TryGetSkill<ScannerSkill>().IsScannerActive)
            {
                StateMachine.TryRequestState<IdleAttackState>();
            }
            _pistol.transform.GetChild(0).LocalResetAll();
            _crowbar.SetActive(false);
            _pistol.SetActive(true);
            _currentAttackSkill = _abilitiesController.TryGetSkill<ProjectileSkill>();
        }

        private void OnDestroy()
        {
            if (_crowbarAnimHandler != null) _crowbarAnimHandler.OnAnimationEnded -= HandleAttackAnimEnd;
            if (_gunAnimHandler != null) _gunAnimHandler.OnAnimationEnded -= HandleAttackAnimEnd;
        }
    }
}
