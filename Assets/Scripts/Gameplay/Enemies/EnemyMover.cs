using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMover : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _playerTransform;

        [SerializeField] private float _rotationSpeed = 5f;



        public void Init(Transform playerTransform)
        {
            _agent = GetComponent<NavMeshAgent>();
            _playerTransform = playerTransform;
        }

        public void SetFollowPlayer()
        {
            if (_playerTransform && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.SetDestination(_playerTransform.position);
                print(_playerTransform.position);
            }
        }

        public void SetRunFromPlayer()
        {
            if (_playerTransform && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.SetDestination(GetRunPoint());
            }
        }

        public void SetDestination(Vector3 target)
        {
            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.SetDestination(target);
            }
        }

        public bool HasReachedDestination()
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Vector3 GetRunPoint() =>
            (transform.position - _playerTransform.position).normalized * 2f + transform.position;

        public void RotateToPlayer()
        {
            if (!_playerTransform) return;
            Vector3 direction = _playerTransform.position - transform.position;
            direction.y = 0; // Игнорируем высоту
            if (direction != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        
        public void Stop()
        {
            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
                _agent.isStopped = true;
        }
        
        public void Resume()
        {
            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
                _agent.isStopped = false;
        }

        public void HardReset()
        {
            if (_agent != null)
            {
                bool wasStopped = _agent.isStopped;
                _agent.enabled = false;
                _agent.enabled = true;
                if (_agent.isOnNavMesh)
                    _agent.isStopped = wasStopped;
            }
        }
    }
}
