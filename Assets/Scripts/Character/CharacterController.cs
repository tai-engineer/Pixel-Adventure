using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour, IDamageable, IDamager, IItemPickUp
{
    #region References
    [Header("Event Raised")]
    [SerializeField] FloatEventChannelSO _hitEvent = default;

    [Space]
    [Header("Inputs")]
    [SerializeField] InputReaderSO _input = default;

    [Space]
    [Header("Stats")]
    [SerializeField] CharacterStatsSO _stats = default;
    #endregion
    #region Raycast
    [Space]
    [Header("Raycast")]
    [Tooltip("Use for ground and ceiling detection")]
    [SerializeField] ContactFilter2D _raycastMask = default;
    [SerializeField] float _groundCastDistance = default;
    [SerializeField] bool _raycastDebug = default;
    RaycastHit2D[] _raycastHits = new RaycastHit2D[3];

    [SerializeField] LayerMask _wallLayer = default;
    [SerializeField] float _wallCheckDistance = default;
    RaycastHit2D _wallHit = new RaycastHit2D();
    #endregion
    #region Movement Variables
    [Space]
    [Header("Physics")]
    [SerializeField] float _gravity = default;
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
    public bool IsWallCollided { get; private set; }
    public float Gravity { get { return _gravity; } }
    public Vector2 FaceDirection { get; private set; } = Vector2.right;
    /// <summary>
    /// Return normalized move vector
    /// </summary>
    public Vector2 Direction { get { return moveVector.normalized; } }

    /// <summary>
    /// Cannot be hurt when invincible
    /// </summary>
    public bool IsInvincible { get; private set; }
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
        Flip();
    }
    public void SetJumpHeight(float height)
    {
        moveVector.y = height;
    }
    public void SetHorizontalDistance(float distance)
    {
        moveVector.x = distance;
    }
    public void GroundHorizontalMovement()
    {
        float desiredSpeed = moveInput.x * _stats.MaxSpeed;
        moveVector.x = Mathf.MoveTowards(moveVector.x, desiredSpeed, _stats.MaxAcceleration * Time.deltaTime);
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
            moveVector.x = Mathf.MoveTowards(moveVector.x, 0f, _stats.MaxAcceleration * Time.deltaTime);
        else
            moveVector.x = Mathf.MoveTowards(moveVector.x, desiredSpeed, _stats.MaxAcceleration * _stats.AirResistance * Time.deltaTime);
    }
    public void ResetMoveVector()
    {
        moveVector = Vector2.zero;
    }    
    void Flip()
    {
        _renderer.flipX = moveVector.x < 0 ? true : moveVector.x > 0 ? false : _renderer.flipX;
        FaceDirection = _renderer.flipX ? Vector2.left : Vector2.right;
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
    public void CheckWallCollided()
    {
        IsWallCollided = false;

        Vector2 size = _box.size * 0.5f;
        Vector2 rightCenter = (Vector2)_box.bounds.center + FaceDirection * size.x;

        RaycastHit2D hit = Physics2D.Raycast(rightCenter, FaceDirection, _wallCheckDistance, _wallLayer);
        _wallHit = hit;
        IsWallCollided = _wallHit.collider != null;
    }
    #endregion
    #region Interface Implementations
    public void TakeDamage(float damage)
    {
        if (IsInvincible)
            return;

        _stats.DecreaseHealth(damage);
        _hitEvent.RaiseEvent(damage);
    }
    public void Damage(GameObject obj, float damage)
    {
        IDamageable damageable = obj.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    public void GainScore(int score)
    {
        _stats.AddScore(score);
    }

    public void GainHealth(float health)
    {
        _stats.IncreaseHealth(health);
    }
    #endregion
    #region Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        Damage(other.gameObject, _stats.Damage);
    }
    #endregion
    #region Effects
    public void StartFading()
    {
        StartCoroutine(FadeInAndOut());
    }
    IEnumerator FadeInAndOut()
    {
        IsInvincible = true;

        float counter = 0f;

        float duration = _stats.FadeDuration;
        float speed = _stats.FadeSpeed;
        int fadeCount = _stats.FadeCount;
        
        Color spirteColor = _renderer.material.color;

        while (fadeCount > 0)
        {
            // Fade in
            while (counter < duration)
            {
                counter += Time.deltaTime * speed;
                spirteColor.a = Mathf.Lerp(1f, 0f, counter / duration);

                _renderer.material.color = spirteColor;
                yield return null;
            }


            counter = 0f;
            // Fade out
            while (counter < duration)
            {
                counter += Time.deltaTime * speed;
                spirteColor.a = Mathf.Lerp(0f, 1f, counter / duration);

                _renderer.material.color = spirteColor;
                yield return null;
            }

            fadeCount--;
        }

        IsInvincible = false;

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

            if (_wallHit.collider != null)
            {
                Gizmos.DrawSphere(_wallHit.point, 0.1f); 

    }
        }
    }

}
