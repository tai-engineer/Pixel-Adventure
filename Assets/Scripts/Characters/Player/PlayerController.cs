using UnityEngine;

namespace PixelAdventure
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] InputReader _playerInput = default;

        [Header("Movement")]
        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector2 moveVector;

        public bool IsWalking { get; private set; } = false;
        public bool IsGrounded { get; private set; } = false;
        public bool JumpInput { get; private set; } = false;
        [Space]
        [Header("Animation parameters")]
        [SerializeField] string _walkingParameter = "";
        [SerializeField] string _airBorneParameter = "";

        [HideInInspector] public int walkingHash;
        [HideInInspector] public int airBorneHash;

        [Space]
        [Header("Gravity")]
        [Tooltip("Negative value which represents gravity")]
        public float verticalPull;

        #region Components
        Vector2 _boxSize;
        Vector2 _boxCenter;
        BoxCollider2D _box;
        SpriteRenderer _renderer;
        #endregion
        void OnEnable()
        {
            _playerInput.moveEvent += OnMoveInitiated;
            _playerInput.moveCanceledEvent += OnMoveCanceled;
            _playerInput.jumpEvent += OnJumpInitiated;
            _playerInput.jumpCanceledEvent += OnJumpCanceled;
        }
        void Awake()
        {
            _box = GetComponent<BoxCollider2D>();
            _renderer = GetComponent<SpriteRenderer>();

            GetParameterHash();
        }

        void Start()
        {
            _boxSize = _box.size;
            _boxCenter = _box.bounds.center;
        }
        void Update()
        {
            IsWalking = moveInput.x != 0;
            IsGrounded = GroundCheck();

            if (_direction != _previousDirection)
                Flip();
        }
        void OnDisable()
        {
            _playerInput.moveEvent -= OnMoveInitiated;
            _playerInput.moveCanceledEvent -= OnMoveCanceled;
            _playerInput.jumpEvent -= OnJumpInitiated;
            _playerInput.jumpCanceledEvent -= OnJumpCanceled;
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
        #region Animator
        void GetParameterHash()
        {
            walkingHash = Animator.StringToHash(_walkingParameter);
            airBorneHash = Animator.StringToHash(_airBorneParameter);
        }
        #endregion
        #region Ground Check
        float radiusY;
        Vector2 bottom;
        bool GroundCheck()
        {
            radiusY = _boxSize.y * 0.5f;
            bottom = new Vector2(_boxCenter.x, _boxCenter.y - radiusY);
            float distance = 0.3f;
            RaycastHit2D hit = Physics2D.Raycast(bottom, new Vector2(0, -1), distance, gameObject.layer);
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
    }

    public enum FaceDirection { Left, Right}
}
