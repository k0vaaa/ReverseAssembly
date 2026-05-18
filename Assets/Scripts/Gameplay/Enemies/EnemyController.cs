using System;
using Core.Bootstrap;

using Core.Events;
using Core.Extensions;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.Combat.Health;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.States;
using Gameplay.Events;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using AttackState = Gameplay.Enemies.States.AttackState;

namespace Gameplay.Enemies
{
    public class EnemyController : StateBehaviourController, ICharacterController, IGlitchable, IInitializable
    {
        
        public bool isDead { get; set; }
        public string UniqueId { get; set; }
        public int PrefabIndex { get; set; }
        Transform ICharacterController.transform => transform;
        GameObject ICharacterController.gameObject => gameObject;
        StabilitySystem ICharacterController.GetComponent<T>() => GetComponent<StabilitySystem>();
        T ICharacterController.GetComponentInChildren<T>() => GetComponentInChildren<T>();

        [SerializeField] private GameObject _sword;

        public Collider SwordCollider { get; private set; }

        private Canvas _hpCanvas;

        private EnemyAnimator _enemyAnimator;

        private StabilitySystem _stabilitySystem;

        private NavMeshAgent _agent;

        private Transform _playerTransform;

        private EnemySkillsController _abilitiesController;

        [SerializeField] private float _rotationSpeed;

        [SerializeField] private float _searchRadius;

        [SerializeField] private float _attackRange;

        public int DeadEnemies;

        public bool IsChasing { get; private set; }

        public bool IsInAttackRange { get; private set; }

        private bool _isPeaceful;

        public bool IsStunned { get; private set; }

        private float _stunEndTime;
        [SerializeField] private AnimationEventsHandler _animHandler;
        
        [Header("Loot")] 
        [SerializeField] private GameObject _codeBlockPrefab;
        [Inject] private Container _container;
        private Container _enemyContainer;
        private bool _attackAnimEnded;

        private void GetComponents()
        {
            SwordCollider = _sword.GetComponent<BoxCollider>();
            _stateMachine = new StateMachine();
            _abilitiesController = GetComponent<EnemySkillsController>();
            _enemyAnimator = GetComponent<EnemyAnimator>();
            _stabilitySystem = GetComponent<StabilitySystem>();
            gameObject.GetComponent<IHittable>().onHit.AddListener(_enemyAnimator.DoHitEvent);
            _agent = GetComponent<NavMeshAgent>();
            _hpCanvas = GetComponentInChildren<Canvas>();
        }

        public void Init()
        {
            EventBus.Subscribe<PlayerSpawnEvent>(HandlePlayerSpawn).AddTo(gameObject);
            _isPeaceful = false;
            _enemyContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(this);
            });
            GetComponents();
            InjectAndInit();
            EnemyStatesInit();
        }

        private void InjectAndInit()
        {
            // todo skill controller needs inject
            
            GameObjectInjector.InjectRecursive(gameObject, _container);
            _enemyAnimator.Init();
            _abilitiesController.Init();
            

        }

        private void HandleAnimEnd()
        {
            _attackAnimEnded = true;
        }

        public void ResetAnim()
        {
            _attackAnimEnded = false;
        }

        private void OnEnable()
        {
            _animHandler.OnAnimationEnded += HandleAnimEnd;
            ForceRequestState<IdleState>();
            
        }

        private void OnDisable()
        {
            _animHandler.OnAnimationEnded -= HandleAnimEnd;
        }

        public void Init(bool isPeaceful, Transform playerTransform)
        {
        }

        public void HandlePlayerSpawn(PlayerSpawnEvent e)
        {
            _playerTransform = e.PlayerTransform;
        }

        private void Update()
        {
            if (!_playerTransform) return;

            if (IsStunned && Time.time >= _stunEndTime)
            {
                IsStunned = false;
            }

            ChasingChecker();
            _stateMachine.Tick();
        }

        public void ApplyGlitchStun(float duration)
        {
            IsStunned = true;
            _stunEndTime = Time.time + duration;
        }

        public void SpawnLoot()
        {
            if (_codeBlockPrefab != null)
            {
                Instantiate(_codeBlockPrefab, transform.position + Vector3.up, Quaternion.identity);
            }
        }

        public void SetFollowPlayer()
        {
            _agent.SetDestination(_playerTransform.position);
        }

        public void SetRunFromPlayer()
        {
            _agent.SetDestination(GetRunPoint());
        }

        private Vector3 GetRunPoint() =>
            (transform.position - _playerTransform.position).normalized * 2f + transform.position;


        public void RotateToPlayer()
        {
            Vector3 direction = _playerTransform.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        

        private void EnemyStatesInit()
        {
            IState attackState;
            Type type;
            if (CompareTag("Wizard"))
            {
                attackState = new RangeAttackState(this, _enemyAnimator, _agent, _abilitiesController);
                type = typeof(ProjectileSkill);
            }
            else
            {
                attackState = new AttackState(this, _enemyAnimator, _agent, _abilitiesController);
                type = typeof(PunchSkill);
            }
            var skill = _abilitiesController.TryGetSkill(type);
            bool AttackReady() => skill.IsReady;


            var idleState = new IdleState(this, _enemyAnimator, _agent);
            var walkState = new WalkState(this, _enemyAnimator, _agent);
            var fearState = new FearState(this, _enemyAnimator, _agent);
            var deathState = new DeathState(this, _enemyAnimator, _agent, _hpCanvas);

            


            _stateMachine.AddAnyTransition(deathState, () => _stabilitySystem.Stability <= 0f);

            var stunState = new GlitchStunState(this, _enemyAnimator, _agent);
            _states[typeof(IdleState)] = idleState;
            _states[typeof(WalkState)] = walkState;
            _states[typeof(FearState)] = fearState;
            _states[typeof(DeathState)] = deathState;
            _states[typeof(AttackState)] = attackState;
            _states[typeof(GlitchStunState)] = stunState;

            _stateMachine.AddAnyTransition(stunState, () => IsStunned);
            _stateMachine.AddTransition(stunState, idleState, () => !IsStunned);

            if (_isPeaceful)
            {
                _stateMachine.AddTransition(idleState, fearState,
                    () => _stabilitySystem.Stability / _stabilitySystem.MaxStability <= .3f);
            }
            else
            {
                _stateMachine.AddTransition(idleState, walkState, () => IsChasing && !IsInAttackRange);
                _stateMachine.AddTransition(idleState, attackState,
                    () => IsInAttackRange && AttackReady());
            }


            _stateMachine.AddTransition(walkState, idleState, () => !IsChasing || IsInAttackRange);
            _stateMachine.AddTransition(walkState, attackState, () => IsInAttackRange && AttackReady());
            _stateMachine.AddTransition(attackState, idleState,
                () => IsInAttackRange && _attackAnimEnded);
            _stateMachine.AddTransition(attackState, walkState,
                () => !IsInAttackRange && _attackAnimEnded);


            // enemyStateMachine.AddTransition(idleState, spellState, () => IsInCastRange && (Time.time - lastAttackTime >= attackCooldown));
            // enemyStateMachine.AddTransition(spellState, idleState, () => IsInCastRange && RangeAnimationEnded());
            // enemyStateMachine.AddTransition(walkState, spellState, () => IsInCastRange);
            // enemyStateMachine.AddTransition(spellState, walkState, () => !IsInCastRange && RangeAnimationEnded());


            _stateMachine.TrySetState(idleState);
        }

        private void ChasingChecker()
        {
            if (Vector3.Distance(_agent.transform.position, _playerTransform.position) <= _searchRadius)
            {
                IsChasing = true;
                if (Vector3.Distance(_agent.transform.position, _playerTransform.position) <= _attackRange)
                {
                    IsInAttackRange = true;
                }
                else
                {
                    IsInAttackRange = false;
                }
            }
            else
            {
                IsChasing = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
        }
    }
}