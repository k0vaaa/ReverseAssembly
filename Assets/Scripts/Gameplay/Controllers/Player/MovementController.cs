using Core.Bootstrap;
using Core.Events;
using Core.Extensions;
using Core.Input;
using Core.StateMachines;
using Gameplay.Anims;
using Gameplay.StateMachines.PlayerStates.MoveStates;
using Gameplay.Combat.Health;
using Gameplay.Events;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class MovementController : StateBehaviourController, IInitializable
    {
        [Inject] private InputManager _inputManager;
        private IPlayerAnimator _playerAnimator;
        private CharacterController _controller;
        private Camera _camera;
        private StabilitySystem _stabilitySystem;

        public bool CanSprint => _stabilitySystem == null ||
                                 (_stabilitySystem.Stability / _stabilitySystem.MaxStability) > 0.3f;


        public float WalkSpeed = 2f;
        public float CurrentMoveSpeed = 2f;

        public float RunSpeed = 2f;
        [field: SerializeField] public bool IsGrounded { get; private set; }

        [SerializeField] private float _velocityVertical = 0f;

        [SerializeField] private float _jumpForce;

        [SerializeField] private float _rotationSpeed = 1f;

        [SerializeField] private LayerMask _groundCheckLayerMask;
        private float _groundCheckDistance;

        private bool _isDead;

        private Vector3 _inertialMoveDirection;


        [Header("Controller")]

        //[SerializeField] private Transform _spine;
        [SerializeField]
        private Transform _foot;

        [SerializeField] private Transform _head;
        [SerializeField] private Transform _root;


        #region States

        private JumpingState _jumpingState;
        private DeathState _deathState;
        private FallingState _fallingState;
        private SprintingState _sprintingState;
        private WalkingState _walkingState;
        private IdleState _idleState;

        #endregion


        private void Awake()
        {
            _stateMachine = new StateMachine();
            _playerAnimator = GetComponent<IPlayerAnimator>();
            _controller = GetComponent<CharacterController>();
            _stabilitySystem = GetComponent<StabilitySystem>();
            _camera = GetComponentInChildren<Camera>();
        }

        /*public void Init(Camera camera)
        {
            if (camera == null)
            {
                _camera = GetComponentInChildren<Camera>();
            }
            else
            {
                _camera = camera;
            }
        }*/

        public void Init()
        {
            _groundCheckDistance = (_controller.height * transform.lossyScale.y / 2) + .1f;

            MoveStatesInit();
        }

        private void Update()
        {
            _stateMachine.Tick();
            //SetControllerParams();
            //Rotate();
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            CheckGround();
        }

        private void MoveStatesInit()
        {
            _idleState = new IdleState(this, _playerAnimator);
            _jumpingState = new JumpingState(this, _playerAnimator);
            _walkingState = new WalkingState(this, _playerAnimator);
            _sprintingState = new SprintingState(this, _playerAnimator);
            _fallingState = new FallingState(this, _playerAnimator);
            _deathState = new DeathState(this, _playerAnimator);
            EventBus.Subscribe<PlayerDeathEvent>(HandleDeath).AddTo(gameObject);

            _stateMachine.AddTransition(_idleState, _walkingState, () => _inputManager.MoveInput != Vector2.zero);
            _stateMachine.AddTransition(_idleState, _sprintingState, () => _inputManager.SprintInput && CanSprint);
            _stateMachine.AddTransition(_walkingState, _idleState, () => _inputManager.MoveInput == Vector2.zero);
            _stateMachine.AddTransition(_walkingState, _sprintingState, () => _inputManager.SprintInput && CanSprint);
            _stateMachine.AddTransition(_walkingState, _fallingState, () => !IsGrounded);
            _stateMachine.AddTransition(_sprintingState, _walkingState, () => !_inputManager.SprintInput || !CanSprint);
            _stateMachine.AddTransition(_sprintingState, _fallingState, () => !IsGrounded);
            _stateMachine.AddTransition(_jumpingState, _fallingState, () => !IsGrounded);
            _stateMachine.AddTransition(_fallingState, _idleState, () => IsGrounded);
            _stateMachine.AddAntiState(_fallingState, _jumpingState);


            if (_inputManager != null)
            {
                _inputManager.OnJumpPressed += HandleJump;
            }

            _stateMachine.TrySetState(_idleState);
        }

        private void HandleJump()
        {
            if (IsGrounded)
            {
                _stateMachine.ForceSetState(_jumpingState);
            }
        }

        private void HandleDeath(PlayerDeathEvent e)
        {
            _stateMachine.ForceSetState(_deathState);
        }

        #region Movement

        private void ApplyGravity()
        {
            if (!_controller.isGrounded)
            {
                _velocityVertical -= 15f * Time.fixedDeltaTime;
                _controller.Move(Vector3.up * (_velocityVertical * Time.fixedDeltaTime));
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
            var radius = _controller.radius * transform.lossyScale.x;
            IsGrounded = Physics.SphereCast(ray, radius, _groundCheckDistance - radius,
                _groundCheckLayerMask);
        }

        #endregion

        private void OnDestroy()
        {
            _inputManager.OnJumpPressed -= HandleJump;
        }

        private void OnDrawGizmos()
        {
            if (_controller == null) return;

            Vector3 origin = transform.TransformPoint(_controller.center);
            var radius = _controller.radius * transform.lossyScale.x;
            float maxDistance = (_controller.height * transform.lossyScale.y / 2) + 0.1f - radius;
            //float maxDistance = 5;

            // Зеленый цвет, если на земле, красный если в воздухе
            Gizmos.color = Color.green;

            // Отрисовка SphereCast
            Gizmos.DrawWireSphere(origin, radius); // Стартовая сфера (внутри тела)
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(origin + Vector3.down * maxDistance,
                radius); // Конечная сфера (возле ног)

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