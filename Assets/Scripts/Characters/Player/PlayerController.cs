using UnityEngine;

namespace PixelAdventure
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] InputReader _playerInput = default;

        [Header("Movement")]
        public float _jumpForce = 5.0f;
        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector2 moveVector;
        bool _jumpInput = false;

        [HideInInspector] public bool isWalking = false;
        [HideInInspector] public bool isAirborne = false;

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


        void OnEnable()
        {
            _playerInput.moveEvent += OnMoveInitiated;
            _playerInput.moveCanceledEvent += OnMoveCanceled;
            _playerInput.jumpEvent += OnJumpInitiated;
            _playerInput.jumpCanceledEvent += OnJumpCanceled;
        }
        void Awake()
        {
            GetParameterHash();
        }

        void OnDisable()
        {
            _playerInput.moveEvent -= OnMoveInitiated;
            _playerInput.moveCanceledEvent -= OnMoveCanceled;
            _playerInput.jumpEvent -= OnJumpInitiated;
            _playerInput.jumpCanceledEvent -= OnJumpCanceled;
        }

        #region Movement
        void OnMoveInitiated(Vector2 input)
        {
            moveInput = input;
        }
        void OnMoveCanceled()
        {
            moveInput = Vector2.zero;
        }
        void OnJumpInitiated()
        {
            _jumpInput = true;
        }
        void OnJumpCanceled()
        {
            _jumpInput = false;
        }
        #endregion
        #region Animator
        void GetParameterHash()
        {
            walkingHash = Animator.StringToHash(_walkingParameter);
            airBorneHash = Animator.StringToHash(_airBorneParameter);
        }
        #endregion
    }
}
