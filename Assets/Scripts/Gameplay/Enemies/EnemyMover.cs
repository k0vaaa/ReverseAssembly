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
                _agent.SetDestination(_playerTransform.position);
        }

        public void SetRunFromPlayer()
        {
            if (_playerTransform && _agent.isActiveAndEnabled && _agent.isOnNavMesh) 
                _agent.SetDestination(GetRunPoint());
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
    }
}
