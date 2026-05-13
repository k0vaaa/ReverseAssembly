using System;
using Core.DI;
using Core.Input;
using Gameplay.Anims;
using Gameplay.Combat.Interfaces;
using Gameplay.StateMachines;
using Gameplay.StateMachines.PlayerStates.MoveStates;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class MovementController : MonoBehaviour, IInjectable
    {
        [Inject] private InputManager _inputManager;
        private StateMachine _moveStateMachine;
        private IPlayerAnimator _playerAnimator;
        private CharacterController _controller;
        private Camera _camera;


        public float WalkSpeed = 2f;
        public float CurrentMoveSpeed = 2f;

        public float RunSpeed = 2f;
        [field: SerializeField] public bool IsGrounded { get; private set; }

        [SerializeField] private float _velocityVertical = 0f;

        [SerializeField] private float _jumpForce;

        [SerializeField] private float _rotationSpeed = 1f;

        private float _groundCheckDistance;

        private bool _death;

        private Vector3 _inertialMoveDirection;

        [Header("Controller")]

        //[SerializeField] private Transform _spine;
        [SerializeField]
        private Transform _foot;

        [SerializeField] private Transform _head;
        [SerializeField] private Transform _root;


        private void Awake()
        {
            _moveStateMachine = new StateMachine();
            _playerAnimator = GetComponent<IPlayerAnimator>();
            _controller = GetComponent<CharacterController>();
        }

        public void Init(Camera camera)
        {
            if (camera == null)
            {
                _camera = GetComponentInChildren<Camera>();
            }
            else
            {
                _camera = camera;
            }
        }

        private void Start()
        {
            _groundCheckDistance = (_controller.height * transform.lossyScale.y / 2) + .1f;

            MoveStatesInit();
        }

        private void Update()
        {
            _moveStateMachine.Tick();
            //SetControllerParams();
            //Rotate();
            ApplyGravity();
            CheckGround();
        }

        private void MoveStatesInit()
        {
            var idleState = new IdleState(this, _playerAnimator);
            var jumpingState = new JumpingState(this, _playerAnimator);
            var walkingState = new WalkingState(this, _playerAnimator);
            var sprintingState = new SprintingState(this, _playerAnimator);
            var fallingState = new FallingState(this, _playerAnimator);
            var deathState = new DeathState(this, _playerAnimator);
            gameObject.GetComponent<IKillable>().OnDeath.AddListener(value => _death = value);

            _moveStateMachine.AddTransition(idleState, walkingState, () => _inputManager.MoveInput != Vector2.zero);
            _moveStateMachine.AddTransition(idleState, sprintingState, () => _inputManager.SprintInput);
            _moveStateMachine.AddTransition(walkingState, idleState, () => _inputManager.MoveInput == Vector2.zero);
            _moveStateMachine.AddTransition(walkingState, sprintingState, () => _inputManager.SprintInput);
            _moveStateMachine.AddTransition(walkingState, fallingState, () => !IsGrounded);
            _moveStateMachine.AddTransition(sprintingState, walkingState, () => !_inputManager.SprintInput);
            _moveStateMachine.AddTransition(sprintingState, fallingState, () => !IsGrounded);
            _moveStateMachine.AddTransition(jumpingState, fallingState, () => !IsGrounded);
            _moveStateMachine.AddTransition(jumpingState, idleState, () => IsGrounded);
            _moveStateMachine.AddTransition(fallingState, idleState, () => IsGrounded);
            _moveStateMachine.AddAntiState(fallingState, jumpingState);


            if (_inputManager != null)
            {
                _inputManager.OnJumpPressed += HandleJump;
            }

            _moveStateMachine.AddAnyTransition(deathState, () => _death);

            _moveStateMachine.SetState(idleState);
        }

        private void HandleJump()
        {
            if (IsGrounded)
            {
                _moveStateMachine.SetState(new JumpingState(this, _playerAnimator));
            }
        }

        private void ApplyGravity()
        {
            if (!_controller.isGrounded)
            {
                _velocityVertical -= 15f * Time.deltaTime;
                _controller.Move(Vector3.up * (_velocityVertical * Time.deltaTime));
            }
            else
            {
                _velocityVertical = 0;
            }
        }

        private void Rotate()
        {
            if (!_camera) return;
            Vector3 forwardDirection = _camera.transform.forward;
            Vector3 rightDirection = _camera.transform.right;
            forwardDirection.y = 0;
            rightDirection.y = 0;
            if (_inputManager.RMBInput)
            {
                Quaternion desiredRotation =
                    Quaternion.LookRotation(forwardDirection, Vector3.up) * Quaternion.Euler(0, 30f, 0);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
            else if (_inputManager.MoveInput != Vector2.zero)
            {
                Vector3 moveDirection = forwardDirection.normalized * _inputManager.MoveInput.y +
                                        rightDirection.normalized * _inputManager.MoveInput.x;
                Quaternion desiredRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
        }


        public void Move()
        {
            Vector3 forwardDirection = _camera.transform.forward;
            Vector3 rightDirection = _camera.transform.right;
            forwardDirection.y = 0;
            rightDirection.y = 0;
            Vector3 moveDirection = forwardDirection.normalized * _inputManager.MoveInput.y +
                                    rightDirection.normalized * _inputManager.MoveInput.x;
            _inertialMoveDirection = moveDirection;

            _controller.Move(moveDirection * (Time.deltaTime * CurrentMoveSpeed));
        }

        public void InertialMove()
        {
            _controller.Move(_inertialMoveDirection * (Time.deltaTime * CurrentMoveSpeed));
        }

        public void Jump()
        {
            _velocityVertical = _jumpForce;
            _controller.Move(Vector3.up * (_velocityVertical * Time.deltaTime));
        }

        private void SetControllerParams()
        {
            var size = _head.position.y - _foot.position.y;
            _controller.height = size + .4f;
            _controller.center = _root.localPosition;
        }


        private void CheckGround()
        {
            Ray ray = new Ray(transform.TransformPoint(_controller.center), Vector3.down);
            IsGrounded = Physics.SphereCast(ray, _controller.radius, _groundCheckDistance,
                LayerMask.GetMask("Ground"));
        }

        private void OnDestroy()
        {
            _inputManager.OnJumpPressed -= HandleJump;
        }

        private void OnDrawGizmos()
        {
            if (_controller == null) return;

            Vector3 origin = transform.TransformPoint(_controller.center);
            float maxDistance = (_controller.height * transform.lossyScale.y / 2) + 0.1f;
            //float maxDistance = 5;

            // Зеленый цвет, если на земле, красный если в воздухе
            Gizmos.color = Color.green;

            // Отрисовка SphereCast
            Gizmos.DrawWireSphere(origin, _controller.radius); // Стартовая сфера (внутри тела)
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(origin + Vector3.down * maxDistance,
                _controller.radius); // Конечная сфера (возле ног)

            // Линии, соединяющие сферы для имитации "капсулы" каста
            Gizmos.DrawLine(origin + Vector3.left * _controller.radius,
                origin + Vector3.down * maxDistance + Vector3.left * _controller.radius);
            Gizmos.DrawLine(origin + Vector3.right * _controller.radius,
                origin + Vector3.down * maxDistance + Vector3.right * _controller.radius);
            Gizmos.DrawLine(origin + Vector3.forward * _controller.radius,
                origin + Vector3.down * maxDistance + Vector3.forward * _controller.radius);
            Gizmos.DrawLine(origin + Vector3.back * _controller.radius,
                origin + Vector3.down * maxDistance + Vector3.back * _controller.radius);
        }
    }
}