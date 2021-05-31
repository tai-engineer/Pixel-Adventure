﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Genera")]
    [Tooltip("True, if sprite face left on the spritesheet. Otherwise false.")]
    [SerializeField] bool _spriteFaceLeft = false;
    [SerializeField] float _moveSpeed = default;
    [SerializeField] LayerMask _ObstacleLayer = default;
    [SerializeField] LayerMask _groundLayer = default;
    [SerializeField] float _gravity = 3f;
    public bool IsGrounded { get; private set; }
    Vector2 _moveVector;
    Vector2 _faceDirection;

    [Space]
    [Header("Scanning Settings")]
    [SerializeField] bool _scanForTarget = false;
    [SerializeField] LayerMask _characterLayer = default;
    [SerializeField] float _viewRadius = default;
    [Range(0f, 360f)]
    [SerializeField] float _viewPOV = default;
    [Range(0, 360f)]
    [SerializeField] float _viewAngle = default;

    Transform _visibleTarget;

    [Space]
    [Header("Range Attack")]
    [SerializeField] bool _hasProjectile = false;
    [SerializeField] float _projectileSpeed = default;
    [SerializeField] Transform _shootingOrigin = default;
    [SerializeField] BulletPool _bulletPool = default;
    [SerializeField] float _timeBetweenShots = default;
    float _rangeAttackTimer = 0f;
    bool _firstShot;

    [Space]
    [Header("Melee Attack")]
    [SerializeField] float _meleeDamage = default;
    [SerializeField] float _timeBetweenMeleeAttack = default;
    float _meleeAttackTimer = 0f;

    #region Comonents
    SpriteRenderer _renderer;
    Rigidbody2D _rb;
    BoxCollider2D _box2D;
    #endregion

    const float RAYCAST_DISTANCE = 0.1f;
    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _box2D = GetComponent<BoxCollider2D>();

        _faceDirection = _spriteFaceLeft ? Vector2.left : Vector2.right;
    }
    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveVector * Time.deltaTime);
        UpdateFacing();
    }
    #region Common
    public void ScanForPlayer()
    {
        if (!_scanForTarget)
            return;

        _visibleTarget = null;

        Vector2 forward = new Vector3(_faceDirection.x, 0f);
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _characterLayer);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(forward, dirToTarget) < _viewPOV /2)
            {
#if UNITY_EDITOR
                Debug.DrawLine(transform.position, target.position); 
#endif
                _visibleTarget = target;
            }
        }
    }

    /// <summary>
    /// Return collider in forward direction
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Collider2D GetForwardCollision(LayerMask layer)
    {
        Vector2 raycastStart = (Vector2)_box2D.bounds.center + _faceDirection * new Vector2(_box2D.size.x * 0.5f, 0f);
        RaycastHit2D hit2D = Physics2D.Raycast(raycastStart, _faceDirection, RAYCAST_DISTANCE, layer);

        return hit2D.collider;
    }
    public void GroundCheck()
    {
        Vector2[] raycastStart = new Vector2[3];
        Vector2 bottomCenter = (Vector2)_box2D.bounds.center + Vector2.down * new Vector2(0f, _box2D.size.y * 0.5f);

        raycastStart[0] = bottomCenter;
        raycastStart[1] = bottomCenter + Vector2.left * new Vector2(_box2D.size.x * 0.5f, 0f);
        raycastStart[2] = bottomCenter + Vector2.right * new Vector2(_box2D.size.x * 0.5f, 0f);

        RaycastHit2D[] hit2D = new RaycastHit2D[3];

        Vector2 normal = Vector2.zero;
        for(int i = 0; i < raycastStart.Length; i++)
        {
            hit2D[i] = Physics2D.Raycast(raycastStart[i], Vector2.down, RAYCAST_DISTANCE, _groundLayer);
            if (hit2D[i].collider != null)
                normal += hit2D[i].normal;
        }
        normal.Normalize();

        if (Mathf.Approximately(normal.x, 0) && Mathf.Approximately(normal.y, 0))
            IsGrounded = false;
        else if(hit2D[0].collider != null)
            IsGrounded = true;
    }
    #endregion
    #region Movement
    public void Move(Vector2 movement)
    {
        _moveVector = movement;
    }

    public Vector2 GetMoveDirection() => _faceDirection.normalized;

    public void UpdateFacing()
    {
        bool faceLeft  = _moveVector.x < 0f;
        bool faceRight = _moveVector.x > 0f;

        if(faceRight)
        {
            _renderer.flipX = _spriteFaceLeft;
            _faceDirection = Vector2.right;
        }
        else if(faceLeft)
        {
            _renderer.flipX = !_spriteFaceLeft;
            _faceDirection = Vector2.left;
        }
    }

    public void FlipMoveDirection()
    {
        _faceDirection.x *= -1f;
    }

    /// <summary>
    /// Turn away when collide obstacle object
    /// </summary>
    public void Patrol()
    {
        if (GetForwardCollision(_ObstacleLayer))
        {
            FlipMoveDirection();
        }

        _moveVector.x = IsGrounded ? _moveSpeed * GetMoveDirection().x : 0f;
        _moveVector.y = Mathf.Max(_moveVector.y - _gravity * Time.deltaTime, -_gravity);

        Move(_moveVector);
    }
    #endregion
    #region Take Damage
    public FloatEvent onTakeDamage = new FloatEvent();
    public void TakeDamage(float damage)
    {
        onTakeDamage.Invoke(damage);
    }
    #endregion
    #region Range Attack
    public void ShootTarget()
    {
        if (!_hasProjectile)
            return;

        if (_visibleTarget == null)
            return;

        if (Time.time - _rangeAttackTimer >= _timeBetweenShots || !_firstShot)
        {
            SpawnProjectiles(_shootingOrigin.position, _visibleTarget.position);
            _firstShot = true;
            _rangeAttackTimer = Time.time;
        }
    }

    public void SpawnProjectiles(Vector2 origin, Vector2 target)
    {
        BulletObject bulletObj = _bulletPool.Pop(_shootingOrigin.position);

        bulletObj.rb2D.velocity = (target - origin).normalized * _projectileSpeed;
    }
    #endregion
    /// <summary>
    /// Shoot first target detected in range
    /// </summary>
    #region Melee Attack
    /// <summary>
    /// Attack when being touch on left/right side
    /// </summary>
    public void TouchAttack()
    {
        Collider2D collider = GetForwardCollision(_characterLayer);
        if (collider != null)
        {
            GameObject obj = collider.gameObject;
            // Hit damageable object
            if (obj.TryGetComponent<IDamageable>(out IDamageable damageable) && (Time.time - _meleeAttackTimer >= _timeBetweenMeleeAttack || _meleeAttackTimer == 0f))
            {
                damageable.TakeDamage(_meleeDamage);
                _meleeAttackTimer = Time.time;
                Debug.Log("Attack");
                return;
            }
        }
    }
    #endregion

    #region Gizmoz
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        DrawConeView();
        DrawView();
    }
    void DrawView()
    {
        Handles.color = new Color(1.0f, 0f, 0f, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, _viewRadius);
    }
    void DrawConeView()
    {
        Vector2 forward = new Vector3(_faceDirection.x, 0f);
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Vector2 endPoint = transform.position + (Quaternion.Euler(0, 0, _viewPOV * 0.5f) * forward);

        Vector2 position = (endPoint - (Vector2)transform.position).normalized;

        Handles.color = new Color(0f, 1f, 0f, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, position, _viewPOV, _viewRadius);
    }
#endif
    #endregion
}
