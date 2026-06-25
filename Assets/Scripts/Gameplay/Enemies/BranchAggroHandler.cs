using Core.Events;
using Core.Extensions;
using Gameplay.Combat.Health;
using Gameplay.Events;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(StabilitySystem), typeof(EnemyBrain))]
    public class BranchAggroHandler : MonoBehaviour
    {
        private StabilitySystem _stabilitySystem;
        private EnemyBrain _brain;
        private NavMeshAgent _agent;
        private Canvas _hpCanvas;
        
        [Tooltip("Аниматор для вызова IdleEvent при переходе в мирный режим")]
        [SerializeField] private EnemyAnimator _enemyAnimator;

        private void Awake()
        {
            _stabilitySystem = GetComponent<StabilitySystem>();
            _brain = GetComponent<EnemyBrain>();
            _agent = GetComponent<NavMeshAgent>();
            _hpCanvas = GetComponentInChildren<Canvas>();
        }

        private void Start()
        {
            if(!enabled) return;
            EventBus.Subscribe<BranchSwitchedEvent>(OnBranchSwitched).AddTo(gameObject);
        }

        public void SetAggressiveMode(bool isAggressive)
        {
            bool isPeaceful = !isAggressive;
            _brain.IsPeaceful = isPeaceful;

            if (isAggressive)
            {
                _stabilitySystem.IsInvincible = false;
                if (_agent != null) _agent.enabled = true;
                if (_hpCanvas != null) _hpCanvas.enabled = true;
                
                // Перезапуск автомата, чтобы он вышел из возможных зависших стейтов
                if (_brain.StateMachine != null) 
                    _brain.StateMachine.ForceRequestState<States.IdleState>();
            }
            else
            {
                if (_agent != null)
                {
                    if (_agent.isActiveAndEnabled) _agent.ResetPath();
                    _agent.enabled = false;
                }
                
                _stabilitySystem.IsInvincible = true;
                if (_hpCanvas != null) _hpCanvas.enabled = false; 
        
                if (_enemyAnimator != null) _enemyAnimator.IdleEvent(); 
            }
        }
        
        private void OnBranchSwitched(BranchSwitchedEvent e)
        {
            bool isMain = e.NewBranch == WorldBranch.Main;
            SetAggressiveMode(isMain);
        }
    }
}
