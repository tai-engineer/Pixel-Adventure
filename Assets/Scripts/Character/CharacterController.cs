using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] InputReaderSO _input = default;
    [SerializeField] CharacterStatsSO _stats = default;
    [SerializeField] float _gravity = default;
    #endregion
    #region Raycast
    [Header("Raycast")]
    [Tooltip("Use for ground and ceiling detection")]
    [SerializeField] ContactFilter2D _raycastMask = default;
    [SerializeField] float _groundCastDistance = default;
    [SerializeField] bool _raycastDebug = default;
    RaycastHit2D[] _raycastHits = new RaycastHit2D[3];
    #endregion
    #region Movement Variables
    [NonSerialized] public Vector2 moveInput;
    [NonSerialized] public Vector2 moveVector;
    #endregion

    #region Getters/Setters
    public bool JumpInput { get; private set; } = false;
    public bool GettingMoveInput { get { return moveInput.x != 0; } }
    public bool IsMoving { get {return moveVector.x != 0f; } }
    public bool IsGrounded { get; private set; } = false;
    public bool IsCeilinged { get; private set; } = false;
    public bool IsFalling { get { return moveVector.y < 0; } }
    public float Gravity { get { return _gravity; } }
    public CharacterStatsSO Stats { get { return _stats; } }
    #endregion

    #region Components
    Rigidbody2D _rb;
    BoxCollider2D _box;
    SpriteRenderer _renderer;
    #endregion
    #region Unity Event Functions
    void Awake()
    {
        _input.moveEvent            += OnMove;
        _input.jumpStartedEvent     += OnJumpStarted;
        _input.jumpCanceledEvent    += OnJumpCanceled;

        _rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Move(moveVector);
    }
    void OnDisable()
    {
        _input.moveEvent            -= OnMove;
        _input.jumpStartedEvent     -= OnJumpStarted;
        _input.jumpCanceledEvent    -= OnJumpCanceled;
    }
    #endregion
    #region Input Events
    void OnMove(Vector2 input)
    {
        moveInput = input;
    }
    void OnJumpStarted()
    {
        JumpInput = true;
    }
    void OnJumpCanceled()
    {
        JumpInput = false;
    }
    #endregion
    #region Movement
    void Move(Vector2 movement)
    {
        moveVector = movement;
        _rb.MovePosition(_rb.position + moveVector * Time.fixedDeltaTime);
    }
    public void SetJumpHeight(float height)
    {
        moveVector.y = height;
    }
    public void GroundHorizontalMovement()
    {
        float desiredSpeed = moveInput.x * _stats.MaxSpeed;
        moveVector.x = Mathf.MoveTowards(moveVector.x, desiredSpeed, _stats.MaxAcceleration * Time.deltaTime);

        Flip();
    }
    public void GroundVerticalMovement()
    {
        if (!IsGrounded)
        {
            moveVector.y = Mathf.MoveTowards(moveVector.y, _stats.MaxFallingForce, Gravity * _stats.FallingAcceleration * Time.deltaTime); 
        }
    }
    public void AirborneVerticalMovement()
    {
        if(IsCeilinged && moveVector.y > 0)
        {
            moveVector.y = 0;
        }

        moveVector.y = Mathf.MoveTowards(moveVector.y, _stats.MaxFallingForce, Gravity * _stats.FallingAcceleration * Time.deltaTime);
    }
    public void AirborneHorizontalMovement()
    {
        float desiredSpeed = moveInput.x * _stats.MaxSpeed;
        if(Mathf.Approximately(desiredSpeed, 0))
            moveVector.x = Mathf.MoveTowards(moveVector.x, desiredSpeed, _stats.MaxAcceleration * Time.deltaTime);
        else
            moveVector.x = Mathf.MoveTowards(moveVector.x, desiredSpeed, _stats.MaxAcceleration * _stats.AirResistance * Time.deltaTime);

        Flip();
    }
    void Flip()
    {
        _renderer.flipX = moveVector.x < 0 ? true : moveVector.x > 0 ? false : _renderer.flipX;
    }
    #endregion
    #region Raycast
    public void CheckGrounded()
    {
        IsGrounded = false;

        Vector2[] raycasts = new Vector2[3];
        Vector2 bottomCenter;
        RaycastHit2D[] hitResults = new RaycastHit2D[3];
        

        Vector2 size = _box.size * 0.5f;
        bottomCenter = (Vector2)_box.bounds.center + Vector2.down * size.y;
        raycasts[0] = bottomCenter + Vector2.left * size.x;
        raycasts[1] = bottomCenter;
        raycasts[2] = bottomCenter + Vector2.right * size.x;

        int count;
        for (int i = 0; i < raycasts.Length; i++)
        {
            count = Physics2D.Raycast(raycasts[i], Vector2.down, _raycastMask, hitResults, _groundCastDistance);
            // Get first contact collider
            _raycastHits[i] = count > 0 ? hitResults[0] : new RaycastHit2D();
            if (_raycastDebug)
            {
                Debug.DrawLine(raycasts[i], raycasts[i] + Vector2.down * _groundCastDistance, Color.red); 
            }
        }

        Vector2 normal = Vector2.zero;
        for (int i = 0; i < _raycastHits.Length; i++)
        {
            if(_raycastHits[i].collider != null)
            {
                normal += _raycastHits[i].normal;
            }
        }

        normal.Normalize();
        if (Mathf.Approximately(normal.x, 0) && Mathf.Approximately(normal.y, 0))
            return;

        if (bottomCenter.y <= _raycastHits[1].point.y + _groundCastDistance * 0.5f)
            IsGrounded = true;
    }
    public void CheckCeiling()
    {
        IsCeilinged = false;

        Vector2[] raycasts = new Vector2[3];
        RaycastHit2D[] hitResults = new RaycastHit2D[3];


        Vector2 size = _box.size * 0.5f;
        Vector2 topCenter = (Vector2)_box.bounds.center + Vector2.up * size.y;
        raycasts[0] = topCenter + Vector2.left * size.x;
        raycasts[1] = topCenter;
        raycasts[2] = topCenter + Vector2.right * size.x;

        int count;
        for (int i = 0; i < raycasts.Length; i++)
        {
            count = Physics2D.Raycast(raycasts[i], Vector2.up, _raycastMask, hitResults, _groundCastDistance);
            // Get first contact collider
            _raycastHits[i] = count > 0 ? hitResults[0] : new RaycastHit2D();
            if (_raycastDebug)
            {
                Debug.DrawLine(raycasts[i], raycasts[i] + Vector2.up * _groundCastDistance, Color.red);
            }
        }

        Vector2 normal = Vector2.zero;
        for (int i = 0; i < _raycastHits.Length; i++)
        {
            if (_raycastHits[i].collider != null)
            {
                normal += _raycastHits[i].normal;
            }
        }

        normal.Normalize();
        if (Mathf.Approximately(normal.x, 0) && Mathf.Approximately(normal.y, 0))
            return;

        if (topCenter.y <= _raycastHits[1].point.y + _groundCastDistance * 0.5f)
            IsCeilinged = true;
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        if(_raycastDebug)
        {
            for (int i = 0; i < _raycastHits.Length; i++)
            {
                if(_raycastHits[i].collider != null)
                {
                    Gizmos.DrawSphere(_raycastHits[i].point, 0.1f);
                }
            }
        }
    }
}
