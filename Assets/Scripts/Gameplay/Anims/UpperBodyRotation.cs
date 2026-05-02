using Core.Input;
using Gameplay.Controllers.Player;
using UnityEngine;

namespace Gameplay.Anims
{
    public class UpperBodyRotation : StateMachineBehaviour
    {
        private InputManager _inputManager;
        private Vector3 _currentLookAtPoint;
        public float rotationSpeed = 5f;
        private bool _isInitialized;
        private float _currentWeight;
        public float weightSpeed = 2f;
        private FightController _fightController;
        private Transform _camera;

        private float _maxArmLength = 0.8f;
        
        
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _inputManager = animator.GetComponent<InputManager>();
            _fightController = animator.GetComponent<FightController>();
            _isInitialized = false;
            _camera = Camera.main.transform;
            Transform head = animator.GetBoneTransform(HumanBodyBones.Head);
            if (head != null)
            {
                _currentLookAtPoint = head.position + head.forward * 10f;
            }

            animator.GetBehaviour<UpperBodyRotation>();
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform head = animator.GetBoneTransform(HumanBodyBones.Head);
            Transform rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            if (head == null || rightShoulder == null) return;

            Vector3 LookDirection()
            {
                var lookDirection = _camera.forward;
                //lookDirection.y = Mathf.Clamp(lookDirection.y, -45f, 60f);
                lookDirection.Normalize();
                return lookDirection;
            }

            Vector3 targetLookDirection = LookDirection();
            Vector3 targetLookAtPoint = _camera.position + targetLookDirection * 100f;
            
            _currentLookAtPoint = Vector3.Lerp(_currentLookAtPoint, targetLookAtPoint, Time.deltaTime * rotationSpeed);

            if (_inputManager.RMBInput && _fightController.IsSheathed)
            {
                if (!_isInitialized)
                {
                    _currentLookAtPoint = targetLookAtPoint;
                    _isInitialized = true;
                }
                _currentWeight = Mathf.Lerp(_currentWeight, 1f, Time.deltaTime * weightSpeed);
            }
            else
            {
                _currentWeight = Mathf.Lerp(_currentWeight, 0f, Time.deltaTime * weightSpeed * 3f);
                _isInitialized = false;
            }

            // Ограничение позиции руки
            Vector3 constrainedHandPosition = ConstrainHandPosition(rightShoulder.position, _currentLookAtPoint, animator);

            animator.SetIKPosition(AvatarIKGoal.RightHand, constrainedHandPosition);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _currentWeight);

            animator.SetLookAtPosition(_currentLookAtPoint);
            animator.SetLookAtWeight(headWeight: _currentWeight, bodyWeight: _currentWeight * 0.25f, weight: _currentWeight);
            
        }

        private Vector3 ConstrainHandPosition(Vector3 shoulderPosition, Vector3 targetPosition, Animator animator)
        {
            // Ограничение углов
            Vector3 shoulderToTarget = targetPosition - shoulderPosition;
            shoulderToTarget.Normalize();
            Vector3 rightDirection = animator.transform.right; // Направление персонажа
            rightDirection.Normalize();
            float angle = Vector3.SignedAngle(rightDirection, shoulderToTarget, animator.transform.up);

            // Ограничиваем угол в диапазоне от -15 до 90 градусов
            float clampedAngle = Mathf.Clamp(angle, -90f, 15f);
            
            Vector3 currentDirection = (targetPosition - shoulderPosition).normalized;
            Vector3 constrainedDirection = Vector3.RotateTowards(rightDirection, -currentDirection, clampedAngle * Mathf.Deg2Rad, 0f);
            constrainedDirection = Vector3.Lerp(constrainedDirection, currentDirection, Time.deltaTime * rotationSpeed);

            
            Vector3 constrainedPosition = shoulderPosition + constrainedDirection * _maxArmLength;

            return constrainedPosition;
        }
    }
}