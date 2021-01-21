using UnityEngine;

namespace PixelAdventure
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D _rb;

        [SerializeField] InputReader _playerInput = default;
        void OnEnable()
        {
            _playerInput.moveEvent += OnMoveInitiated;
            _playerInput.moveCanceledEvent += OnMoveCanceled;
            _playerInput.jumpEvent += OnJumpInitiated;
            _playerInput.jumpCanceledEvent += OnJumpCanceled;
        }
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            _moveVector.y = _jumpInput ? 1.0f : 0.0f;
            _moveVector.y *= _jumpForce;

            _rb.MovePosition(_rb.position + _moveVector * Time.fixedDeltaTime);
        }

        void OnDisable()
        {
            _playerInput.moveEvent -= OnMoveInitiated;
            _playerInput.moveCanceledEvent -= OnMoveCanceled;
            _playerInput.jumpEvent -= OnJumpInitiated;
            _playerInput.jumpCanceledEvent -= OnJumpCanceled;
        }

        #region Movement
        Vector2 _moveVector;
        bool _jumpInput = false;
        [SerializeField] float _moveSpeed = 5.0f;
        [SerializeField] float _jumpForce = 5.0f;
        void OnMoveInitiated(Vector2 moveInput)
        {
            _moveVector.x = moveInput.x * _moveSpeed;
            _moveVector.y = 0.0f;
        }
        void OnMoveCanceled()
        {
            _moveVector = Vector2.zero;
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
    }
}
