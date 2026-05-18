using Core.DI;
using Core.Input;
using Gameplay.Anims;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.StateMachines;
using Gameplay.StateMachines.PlayerStates.AbilityStates;
using Gameplay.StateMachines.PlayerStates.FightStates;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class FightController : MonoBehaviour, IInjectable
    {
        private StateMachine _fightStateMachine;
        private IPlayerAnimator _playerAnimator;
        private SkillsController _skillsController;
        [Inject] private InputManager _inputManager;

        [HideInInspector] public bool IsSheathed;

        [SerializeField] public GameObject swordGameObject;

        [SerializeField] public GameObject hipSwordGameObject;

        #region States

        private AttackState _attackState;
        private SpellState _spellState;
        private IdleAttackState _idleAttackState;
        private SheathedSwordState _sheathState;
        private SwitchBranchAbilityState _switchBranchState;
        private ScannerAbilityState _scannerState;

        #endregion

        //public Sword Sword { get; private set; }
        public Collider SwordCollider { get; private set; }


        private void Awake()
        {
            _fightStateMachine = new StateMachine();
            _skillsController = GetComponent<SkillsController>();
            _playerAnimator = GetComponent<IPlayerAnimator>();
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
            //_attackState = new AttackState(this, _skillsController, _playerAnimator);
            //_spellState = new SpellState(this, _skillsController, _playerAnimator);
            //_sheathState = new SheathedSwordState(this, _skillsController, _playerAnimator);
            _idleAttackState = new IdleAttackState(this, _skillsController, _playerAnimator);
            _switchBranchState = new SwitchBranchAbilityState(this, _skillsController, _playerAnimator);
            _scannerState = new ScannerAbilityState(this, _skillsController, _playerAnimator);


            //bool MeleeAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.99f, "Attack");
            //bool MeleeAnimationEnded() => _attackState.IsFinished;
            //bool SpellAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.99f, "Spell");
            //bool SheathAnimationEnded() => _playerAnimator.CheckAnimationState((int)LayerNames.Fight, 0.99f, "Sheath");


            //_fightStateMachine.AddTransition(idleAttackState,sheathState, () => _inputManager.IsSheathed);
            //_fightStateMachine.AddTransition(sheathState,idleAttackState, () => !_inputManager.IsSheathed);

            //_fightStateMachine.AddTransition(idleAttackState, attackState, () => _inputManager.MeleeInput && _skillsController.Skills[SkillType.Melee]._isReady);
            //TODO подумать над реализацией сканера через стейты
            _inputManager.OnScannerPressed += HandleScannerPress;
            _inputManager.OnInteractPressed += HandleInteractPress;
            
            //_fightStateMachine.AddTransition(idleAttackState, spellState,() => _inputManager.MeleeInput && _skillsController.Skills[SkillType.Fireball]._isReady);
            
            //_fightStateMachine.AddTransition(_attackState, _idleAttackState, MeleeAnimationEnded);
            _fightStateMachine.AddTransition(_switchBranchState, _idleAttackState, () => _switchBranchState.IsFinished);
            _fightStateMachine.AddTransition(_scannerState, _idleAttackState, () => _scannerState.IsFinished);
            
            //_fightStateMachine.AddTransition(spellState,idleAttackState, SpellAnimationEnded);

            //_fightStateMachine.AddTransition(sheathState,spellState, () => _inputManager.RMBInput && _inputManager.SpellInput && _skillsController.Skills[SkillType.Fireball]._isReady);

            _fightStateMachine.SetState(_idleAttackState);
        }

        private void HandleScannerPress()
        {
            if (_skillsController.Skills[SkillType.Scanner]._isReady)
            {
                _fightStateMachine.SetState(_scannerState);
            }
        }
        
        private void HandleInteractPress()
        {
            if (_skillsController.Skills[SkillType.BranchSwitch]._isReady)
            {
                _fightStateMachine.SetState(_switchBranchState);
            }
        }

        private void OnDestroy()
        {
            _inputManager.OnScannerPressed -= HandleScannerPress;
            _inputManager.OnInteractPressed -= HandleInteractPress;
        }
    }
}
