using UnityEngine;

namespace PixelAdventure
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(StateMachine))]
    public class PlayerController : CharacterController
    {
        [Space]
        [Header("Input")]
        [SerializeField] InputReader _input = default;

        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector2 moveVector;

        public bool IsWalking { get; private set; } = false;
        public bool IsGrounded { get; private set; } = false;
        public bool IsAirborne { get; set; } = false;
        public bool JumpInput { get; private set; }

        [Space]
        [Header("Ground")]
        [Tooltip("Layer for ground check")]
        public LayerMask groundLayer;

        #region Components
        BoxCollider2D _box;
        SpriteRenderer _renderer;
        Rigidbody2D _rb;

        StateMachine _stateMachine;
        #endregion
        void OnEnable()
        {
            _input.moveEvent += OnMoveInitiated;
            _input.moveCanceledEvent += OnMoveCanceled;
            _input.jumpEvent += OnJumpInitiated;
            _input.jumpCanceledEvent += OnJumpCanceled;
        }
        void Awake()
        {
            _box = GetComponent<BoxCollider2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _stateMachine = GetComponent<StateMachine>();
        }
        void Update()
        {
            IsWalking = moveInput.x != 0;
            IsGrounded = GroundCheck();

            if (_direction != _previousDirection)
                Flip();

            _stateMachine.OnUpdate();
        }

        void FixedUpdate()
        {
            _stateMachine.OnFixedUpdate();
            Move(moveVector);
        }
        void OnDisable()
        {
            _input.moveEvent -= OnMoveInitiated;
            _input.moveCanceledEvent -= OnMoveCanceled;
            _input.jumpEvent -= OnJumpInitiated;
            _input.jumpCanceledEvent -= OnJumpCanceled;
        }

        protected override void Move(Vector2 nextMovement)
        {
            _rb.MovePosition(_rb.position + nextMovement * Time.fixedDeltaTime);
        }
        #region Handle Movement Event
        void OnMoveInitiated(Vector2 input)
        {
            moveInput = input;
            _direction = moveInput.x < 0 ? FaceDirection.Left : FaceDirection.Right;
        }
        void OnMoveCanceled()
        {
            moveInput = Vector2.zero;
        }
        #endregion
        #region Handle Jump Event
        void OnJumpInitiated()
        {
            JumpInput = true;
        }
        void OnJumpCanceled()
        {
            JumpInput = false;
        }
        #endregion
        #region Ground Check
        float _radiusY;
        Vector2 _bottom;
        float _distance = 0.03f;
        bool GroundCheck()
        {
            _radiusY = _box.size.y * 0.5f;
            _bottom = new Vector2(_box.bounds.center.x, _box.bounds.center.y - _radiusY);
            RaycastHit2D hit = Physics2D.Raycast(_bottom, new Vector2(0, -1), _distance, groundLayer);
            return hit.collider != null;
        }
        #endregion
        #region Flip Sprite
        FaceDirection _direction = FaceDirection.Right;
        FaceDirection _previousDirection = FaceDirection.Right;
        void Flip()
        {
            _renderer.flipX = _direction == FaceDirection.Right ? false : true;
            _previousDirection = _direction;
        }
        #endregion
        #region Gizmos
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(_bottom, new Vector2(0, -1 * _distance));
        }
        #endregion
    }

    public enum FaceDirection { Left, Right}
}
