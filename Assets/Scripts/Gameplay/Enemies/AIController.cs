using System;
using Core.Bootstrap;
using Core.Events;
using Core.Extensions;
using Gameplay.Combat.Health;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Skills;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.Behaviors;
using Gameplay.Events;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(StabilitySystem), typeof(EnemyBrain), typeof(EnemyMover))]
    [RequireComponent(typeof(AbilitiesController))]
    public class AIController : MonoBehaviour, ICharacterController, IInitializable
    {
        [SerializeField, HideInInspector] private string _uniqueId;
        public bool isDead { get; set; }

        public string UniqueId 
        { 
            get => _uniqueId; 
            set => _uniqueId = value; 
        }
        
        public int PrefabIndex { get; set; }

        Transform ICharacterController.transform => transform;
        GameObject ICharacterController.gameObject => gameObject;
        StabilitySystem ICharacterController.GetComponent<T>() => GetComponent<StabilitySystem>();
        T ICharacterController.GetComponentInChildren<T>() => GetComponentInChildren<T>();

        public StabilitySystem StabilitySystem { get; private set; }
        public AbilitiesController AbilitiesController { get; private set; }
        public EnemyBrain Brain { get; private set; }
        public EnemyMover Mover { get; private set; }

        [Inject] protected Container _container;
        protected Container _enemyContainer;

        protected Transform _playerTransform;
        protected bool _isPeaceful;

        [Header("AI Settings")]
        [SerializeField] private EnemyBehaviorStrategy _behaviorStrategy;
        [SerializeField] private float _searchRadius = 10f;
        [SerializeField] private float _attackRange = 2f;
        
        public virtual void Init()
        {
            GetComponents();
            _enemyContainer = _container.Scope(builder =>
            {
                builder.RegisterValue(this);
                builder.RegisterValue(StabilitySystem);
                builder.RegisterValue(AbilitiesController);
                builder.RegisterValue(Brain);
                builder.RegisterValue(Mover);
            });
            GameObjectInjector.InjectRecursive(gameObject, _container);
            
            AbilitiesController.Init();

            EventBus.Subscribe<PlayerSpawnEvent>(HandlePlayerSpawn).AddTo(gameObject);

            if (_playerTransform != null)
            {
                InitializeAI();
            }
        }

        private void GetComponents()
        {
            StabilitySystem = GetComponent<StabilitySystem>();
            AbilitiesController = GetComponent<AbilitiesController>();
            Brain = GetComponent<EnemyBrain>();
            Mover = GetComponent<EnemyMover>();
        }

        public virtual void Init(bool isPeaceful, Transform playerTransform)
        {
            _isPeaceful = isPeaceful;
            _playerTransform = playerTransform;
            Mover.Init(playerTransform);
        }

        private void InitializeAI()
        {
            Brain.Init(Mover, _playerTransform, _searchRadius, _attackRange, _isPeaceful);
            
            if (_behaviorStrategy != null)
            {
                _behaviorStrategy.InitializeBehavior(Brain, this);
            }
            else
            {
                Debug.LogWarning($"[AIController] No behavior strategy assigned to {gameObject.name}!");
            }
        }

        private void HandlePlayerSpawn(PlayerSpawnEvent e)
        {
            _playerTransform = e.PlayerTransform;
            Mover.Init(_playerTransform);
            InitializeAI();
        }

        private void OnEnable()
        {
            if (Brain != null && Brain.StateMachine != null) 
                Brain.StateMachine.ForceRequestState<States.IdleState>();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_uniqueId))
            {
                _uniqueId = Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
        

        protected virtual void OnDestroy()
        {
            _enemyContainer?.Dispose();
        }
    }
}
