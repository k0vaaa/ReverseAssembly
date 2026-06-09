using Core.StateMachines;
using Gameplay.Combat.Interfaces;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public class EnemyBrain : StateBehaviourController, IGlitchable
    {
        private Transform _playerTransform;
        private EnemyMover _mover;
        private float _searchRadius;
        private float _attackRange;
        
        public bool IsChasing { get; private set; }
        public bool IsInAttackRange { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsPeaceful { get; set; }
        
        private float _stunEndTime;

        public void Init(EnemyMover mover, Transform playerTransform, float searchRadius, float attackRange, bool isPeaceful)
        {
            StateMachine = new StateMachine();
            _mover = mover;
            _playerTransform = playerTransform;
            _searchRadius = searchRadius;
            _attackRange = attackRange;
            IsPeaceful = isPeaceful;
        }

        private void Update()
        {
            if (!_playerTransform) return;
            
            if (IsStunned && Time.time >= _stunEndTime)
            {
                IsStunned = false;
            }

            if (!IsPeaceful)
            {
                ChasingChecker();
            }
            
            StateMachine.Tick();
        }

        public void ApplyGlitchStun(float duration)
        {
            IsStunned = true;
            _stunEndTime = Time.time + duration;
        }

        private void ChasingChecker()
        {
            if (Vector3.Distance(_mover.transform.position, _playerTransform.position) <= _searchRadius)
            {
                IsChasing = true;
                IsInAttackRange = Vector3.Distance(_mover.transform.position, _playerTransform.position) <= _attackRange;
            }
            else
            {
                IsChasing = false;
                IsInAttackRange = false;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
