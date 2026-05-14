using Gameplay.Combat.Health;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Controllers.Player;
using Gameplay.Enemies.States;
using Gameplay.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using AttackState = Gameplay.Enemies.States.AttackState;

namespace Gameplay.Enemies
{
    public class EnemyController : MonoBehaviour, ICharacterController
    {   
        
        // public bool isDead { get; set; }
        // public string UniqueId { get; set; }
        // public int PrefabIndex { get; set; }
        // Transform ICharacterController.transform => transform;
        // GameObject ICharacterController.gameObject => gameObject;
        // HealthSystem ICharacterController.GetComponent<T>() => GetComponent<HealthSystem>();
        // T ICharacterController.GetComponentInChildren<T>() => GetComponentInChildren<T>();
        
        public bool isDead { get; set; }
        public string UniqueId { get; set; }
        public int PrefabIndex { get; set; }
        Transform ICharacterController.transform => transform;
        GameObject ICharacterController.gameObject => gameObject;
        StabilitySystem ICharacterController.GetComponent<T>() => GetComponent<StabilitySystem>();
        T ICharacterController.GetComponentInChildren<T>() => GetComponentInChildren<T>();
        
        
        
        
        
        
        
        
        [SerializeField] private GameObject _sword;
        
        public Collider SwordCollider { get; private set; }
        // public string UniqueId;
        // public int PrefabIndex;
        private Canvas hpCanvas;
        private StateMachine enemyStateMachine;
        private EnemyAnimator enemyAnimator;
        private StabilitySystem stabilitySystem;
        private NavMeshAgent _agent;
        private Transform _playerTransform;
        private SkillsController _skillsController;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float searchRadius;
        [SerializeField] private float attackRange;
        public int deadEnemies;
        public bool IsChasing { get; private set;}
        // public bool isDead;
        public bool IsInAttackRange { get; private set;}
        private bool _isPeaceful;
        private void Awake()
        {
            SwordCollider = _sword.GetComponent<BoxCollider>();
            enemyStateMachine = new StateMachine();
            _skillsController = GetComponent<SkillsController>();
            enemyAnimator = GetComponent<EnemyAnimator>();
            stabilitySystem = GetComponent<StabilitySystem>();
            gameObject.GetComponent<IHittable>().onHit.AddListener(enemyAnimator.DoHitEvent);
            _agent = GetComponent<NavMeshAgent>();
            hpCanvas = GetComponentInChildren<Canvas>();
        }

        public void Init(bool isPeaceful, Transform playerTransform)
        {
            _isPeaceful = isPeaceful;
            _playerTransform = playerTransform;
            EnemyStatesInit();
        }

        private void Update()
        {   
            if(!_playerTransform) return;
            ChasingChecker();
            enemyStateMachine.Tick();
        }

        public void SetFollowPlayer()
        {
            _agent.SetDestination(_playerTransform.position);
        }

        public void SetRunFromPlayer()
        {
            _agent.SetDestination(GetRunPoint());
        }

        private Vector3 GetRunPoint() => (transform.position - _playerTransform.position).normalized * 2f + transform.position;
 

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
                attackState = new RangeAttackState(this, enemyAnimator, _agent, _skillsController);
                type = SkillType.Fireball;
            }
            else
            {
                attackState = new AttackState(this, enemyAnimator, _agent, _skillsController);
                type = SkillType.Melee;
            }
            bool AttackReady() => _skillsController.Skills[type]._isReady;
            
            
            
            var idleState = new IdleState(this,enemyAnimator, _agent);
            var walkState =  new WalkState(this,enemyAnimator,_agent);
            var fearState =  new FearState(this,enemyAnimator,_agent);
            var deathState = new DeathState(this,enemyAnimator,_agent, hpCanvas);
            
            bool AttackAnimationEnded() => enemyAnimator.CheckAnimationState(0,1f,"attackTest");


            enemyStateMachine.AddAnyTransition(deathState, () => stabilitySystem.Stability <= 0f);

            if (_isPeaceful)
            {
                enemyStateMachine.AddTransition(idleState, fearState, () => stabilitySystem.Stability/stabilitySystem.MaxStability <= .3f);
            }
            else
            {
                enemyStateMachine.AddTransition(idleState, walkState, () => IsChasing && !IsInAttackRange);
                enemyStateMachine.AddTransition(idleState, attackState,
                    () => IsInAttackRange && AttackReady());
            }
            
            
            enemyStateMachine.AddTransition(walkState, idleState, () => !IsChasing || IsInAttackRange);
            enemyStateMachine.AddTransition(walkState, attackState, () => IsInAttackRange && AttackReady());
            enemyStateMachine.AddTransition(attackState, idleState,
                () => IsInAttackRange && AttackAnimationEnded());
            enemyStateMachine.AddTransition(attackState, walkState,
                () => !IsInAttackRange && AttackAnimationEnded());


            // enemyStateMachine.AddTransition(idleState, spellState, () => IsInCastRange && (Time.time - lastAttackTime >= attackCooldown));
            // enemyStateMachine.AddTransition(spellState, idleState, () => IsInCastRange && RangeAnimationEnded());
            // enemyStateMachine.AddTransition(walkState, spellState, () => IsInCastRange);
            // enemyStateMachine.AddTransition(spellState, walkState, () => !IsInCastRange && RangeAnimationEnded());
            
            
            
            
            enemyStateMachine.SetState(idleState);
        }
        
        private void ChasingChecker()
        {
            if (Vector3.Distance(_agent.transform.position, _playerTransform.position) <= searchRadius)
            {   
                IsChasing = true;
                if (Vector3.Distance(_agent.transform.position, _playerTransform.position) <= attackRange)
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
        
        
}
}
