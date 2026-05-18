using System;
using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Gameplay.Combat.Health;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.States;
using Gameplay.Events;
using Gameplay.StateMachines;
using Gameplay.UI.Views.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using AttackState = Gameplay.Enemies.States.AttackState;

namespace Gameplay.Enemies
{
    public class EnemyController : MonoBehaviour, ICharacterController, IGlitchable, IInitializable, IInjectable
    {
        [Inject] private DIContainer _diContainer;
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

        private StateMachine _enemyStateMachine;

        private EnemyAnimator _enemyAnimator;

        private StabilitySystem _stabilitySystem;

        private NavMeshAgent _agent;

        private Transform _playerTransform;

        private SkillsController _skillsController;

        [SerializeField] private float _rotationSpeed;

        [SerializeField] private float _searchRadius;

        [SerializeField] private float _attackRange;

        public int DeadEnemies;

        public bool IsChasing { get; private set; }

        public bool IsInAttackRange { get; private set; }

        private bool _isPeaceful;

        public bool IsStunned { get; private set; }

        private float _stunEndTime;


        [Header("Loot")] [SerializeField] private GameObject _codeBlockPrefab;

        private void GetComponents()
        {
            SwordCollider = _sword.GetComponent<BoxCollider>();
            _enemyStateMachine = new StateMachine();
            _skillsController = GetComponent<SkillsController>();
            _enemyAnimator = GetComponent<EnemyAnimator>();
            _stabilitySystem = GetComponent<StabilitySystem>();
            gameObject.GetComponent<IHittable>().onHit.AddListener(_enemyAnimator.DoHitEvent);
            _agent = GetComponent<NavMeshAgent>();
            _hpCanvas = GetComponentInChildren<Canvas>();
        }

        public void Init()
        {
            EventBus.Subscribe<PlayerSpawnEvent>(HandlePlayerSpawn).AddTo(gameObject);
            EventBus.Subscribe<BranchSwitchedEvent>(OnBranchSwitched).AddTo(gameObject);
            _isPeaceful = false;
            GetComponents();
            InjectAndInit();
            EnemyStatesInit();
            UpdateUIReference();
        }

        private void InjectAndInit()
        {
            _diContainer.Inject(_skillsController);
            _enemyAnimator.Init();
            _skillsController.Init(null);
            UpdateUIReference();

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
            if (_isPeaceful) return; 

            if (IsStunned && Time.time >= _stunEndTime)
            {
                IsStunned = false;
            }

            ChasingChecker();
            _enemyStateMachine.Tick();
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
            SkillType type;
            if (CompareTag("Wizard"))
            {
                attackState = new RangeAttackState(this, _enemyAnimator, _agent, _skillsController);
                type = SkillType.Fireball;
            }
            else
            {
                attackState = new AttackState(this, _enemyAnimator, _agent, _skillsController);
                type = SkillType.Melee;
            }

            bool AttackReady() => _skillsController.Skills[type]._isReady;


            var idleState = new IdleState(this, _enemyAnimator, _agent);
            var walkState = new WalkState(this, _enemyAnimator, _agent);
            var fearState = new FearState(this, _enemyAnimator, _agent);
            var deathState = new DeathState(this, _enemyAnimator, _agent, _hpCanvas);

            bool AttackAnimationEnded() => _enemyAnimator.CheckAnimationState(0, 0.99f, "PunchEnemyMain");


            _enemyStateMachine.AddAnyTransition(deathState, () => _stabilitySystem.Stability <= 0f);

            var stunState = new GlitchStunState(this, _enemyAnimator, _agent);
            _enemyStateMachine.AddAnyTransition(stunState, () => IsStunned);
            _enemyStateMachine.AddTransition(stunState, idleState, () => !IsStunned);

            if (_isPeaceful)
            {
                _enemyStateMachine.AddTransition(idleState, fearState,
                    () => _stabilitySystem.Stability / _stabilitySystem.MaxStability <= .3f);
            }
            else
            {
                _enemyStateMachine.AddTransition(idleState, walkState, () => IsChasing && !IsInAttackRange);
                _enemyStateMachine.AddTransition(idleState, attackState,
                    () => IsInAttackRange && AttackReady());
            }


            _enemyStateMachine.AddTransition(walkState, idleState, () => !IsChasing || IsInAttackRange);
            _enemyStateMachine.AddTransition(walkState, attackState, () => IsInAttackRange && AttackReady());
            _enemyStateMachine.AddTransition(attackState, idleState,
                () => IsInAttackRange && AttackAnimationEnded());
            _enemyStateMachine.AddTransition(attackState, walkState,
                () => !IsInAttackRange && AttackAnimationEnded());


            // enemyStateMachine.AddTransition(idleState, spellState, () => IsInCastRange && (Time.time - lastAttackTime >= attackCooldown));
            // enemyStateMachine.AddTransition(spellState, idleState, () => IsInCastRange && RangeAnimationEnded());
            // enemyStateMachine.AddTransition(walkState, spellState, () => IsInCastRange);
            // enemyStateMachine.AddTransition(spellState, walkState, () => !IsInCastRange && RangeAnimationEnded());


            _enemyStateMachine.SetState(idleState);
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
        
        
        public void SetAggressiveMode(bool isAggressive)
        {
            if (isDead) return;

            _isPeaceful = !isAggressive;

            if (isAggressive)
            {
                // Логика обычного мира
                _agent.enabled = true;
                if (_hpCanvas != null) _hpCanvas.enabled = true;
                _enemyAnimator.IdleEvent(); // Сбрасываем ходьбу
                _enemyStateMachine.SetState(new IdleState(this, _enemyAnimator, _agent));
            }
            else
            {
                // Логика альтернативного мира (застывание)
                if (_agent.isActiveAndEnabled) _agent.ResetPath();
                _agent.enabled = false;
        
                if (_hpCanvas != null) _hpCanvas.enabled = false; // Отключаем полоску здоровья
        
                _enemyAnimator.IdleEvent(); // Принудительно ставим Idle
            }
        }
        
        private void OnBranchSwitched(BranchSwitchedEvent e)
        {
            bool isMain = e.NewBranch == Gameplay.Events.WorldBranch.Main;
            SetAggressiveMode(isMain);
        }
        
        private void UpdateUIReference()
        {
            // 1. Ищем полоску здоровья внутри этого объекта (на самом враге или его детях)
            var healthBarView = GetComponentInChildren<StabilityBarView>();
    
            // 2. Если полоска найдена, привязываем её к системе стабильности
            if (healthBarView != null)
            {
                // Удаляем все старые подписки (чтобы не было дублей)
                _stabilitySystem.onStabilityChanged.RemoveAllListeners();
        
                // Подписываем полоску на событие изменения стабильности
                _stabilitySystem.onStabilityChanged.AddListener(healthBarView.ChangeHp);
        
                // Обновляем значение полоски сразу, чтобы она не была пустой
                healthBarView.ChangeHp(_stabilitySystem.Stability, _stabilitySystem.MaxStability);
            }
        }
        
    }
}