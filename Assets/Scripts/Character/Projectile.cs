using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _launchAngle = default;
    [SerializeField] int _totalLaunch = 2;
    [SerializeField] LayerMask _groundLayer = default;
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] float _damage = 1f;

    Rigidbody2D _rb;
    Animator _animator;
    AudioSource _audioSource;

    Vector2 _targetPosition;
    float _distanceToTarget;
    int _launchCount = 0;
    float _lifeTimerCounter = 0f;
    Vector2 _direction = Vector2.right;
    public PlayerProjectileObject projectileObject;
    void OnEnable()
    {
        _launchCount = _totalLaunch;
        _lifeTimerCounter = 0f;
        _lifeTimerCounter = Time.time;
    }

    void OnDisable()
    {
        _animator.SetBool("IsExploded", false);
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.time - _lifeTimerCounter > _lifeTime)
        {
            SelfDestroy();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskHelper.Contains(_groundLayer, collision.gameObject))
        {
            Relaunch(new Vector2(_rb.position.x +_distanceToTarget, _targetPosition.y));
        }

        if(collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
            _animator.SetBool("IsExploded", true);
        }
    }
    public void Launch(Vector2 target)
    {
        if (_launchCount <= 0)
        {
            SelfDestroy();
        }

        _rb.velocity = CalculateVelocity(target);


        _launchCount--;
        _targetPosition = target;
        _distanceToTarget = Vector2.Distance(_rb.position, target);
    }

    
    /// <summary>
    /// Returns global velocity to target
    /// </summary>
    /// <returns></returns>
    Vector2 CalculateVelocity(Vector2 target)
    {
        // V * V = G * R * R / (2 * ( H - R * tanAlpha))
        // Ref: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
        float R = Vector2.Distance(_rb.position, target);
        float G = Physics2D.gravity.y;
        float tanAlpha = Mathf.Tan(_launchAngle * Mathf.Deg2Rad);
        float H = target.y - _rb.position.y;

        float Vx = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vx;

        Vector2 localVelocity = new Vector2(Vx * _direction.x, Vy);
        Vector2 globalVelocity = transform.TransformDirection(localVelocity);
        return globalVelocity;
    }
    void Relaunch(Vector2 target)
    {
        Launch(target);
    }
    public void SelfDestroy()
    {
        projectileObject.ReturnToPool();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    public void PlaySoundFX()
    {
        _audioSource.Play();
    }
}
