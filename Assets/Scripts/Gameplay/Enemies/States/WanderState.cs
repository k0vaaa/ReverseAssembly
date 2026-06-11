using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class WanderState : EnemyState
    {
        private readonly float _wanderRadius;
        private readonly float _minWaitTime;
        private readonly float _maxWaitTime;
        private readonly Vector3 _anchorPosition;

        private bool _isWaiting;
        private float _waitTimer;

        public WanderState(AIController controller, EnemyAnimator animator, EnemyMover mover, float wanderRadius, float minWaitTime, float maxWaitTime, Vector3 anchorPosition) 
            : base(controller, animator, mover)
        {
            _wanderRadius = wanderRadius;
            _minWaitTime = minWaitTime;
            _maxWaitTime = maxWaitTime;
            _anchorPosition = anchorPosition;
        }

        protected override void EnterAction()
        {
            SetNewDestination();
        }

        protected override void ExecuteAction()
        {
            if (_isWaiting)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0)
                {
                    SetNewDestination();
                }
            }
            else
            {
                if (Mover.HasReachedDestination())
                {
                    StartWaiting();
                }
            }
        }

        protected override void ExitAction()
        {
            Mover.Stop();
        }

        private void SetNewDestination()
        {
            _isWaiting = false;
            
            // Найти случайную точку вокруг якоря
            Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection.y = 0; // Игнорируем сильные изменения высоты для поиска
            randomDirection += _anchorPosition;
            
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius, NavMesh.AllAreas))
            {
                Mover.SetDestination(hit.position);
                Mover.Resume();
                EnemyAnimator.WalkEvent();
            }
            else
            {
                // Если точку не нашли, просто ждем
                StartWaiting();
            }
        }

        private void StartWaiting()
        {
            _isWaiting = true;
            _waitTimer = Random.Range(_minWaitTime, _maxWaitTime);
            Mover.Stop();
            EnemyAnimator.IdleEvent();
        }
    }
}
