using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif
public class EnemyController : MonoBehaviour
{
    [Header("Scanning Settings")]
    [SerializeField] LayerMask _characterLayer = default;
    [SerializeField] float _viewRadius = default;
    [Range(0f, 360f)]
    [SerializeField] float _viewPOV = default;
    [Range(0, 360)]
    [SerializeField] float _viewAngle = default;

    Transform _visibleTarget;

    [Space]
    [Header("Range Attack")]
    [SerializeField] float _projectileSpeed = default;
    [SerializeField] Transform _shootingOrigin = default;
    [SerializeField] BulletPool _bulletPool = default;
    [SerializeField] float _timeBetweenShots = default;
    float _timer;
    bool _firstShot;

    void Update()
    {
        ScanForPlayer();
        ShootTarget();
    }

    /// <summary>
    /// Shoot first target in range
    /// </summary>
    void ScanForPlayer()
    {
        _visibleTarget = null;

        Vector2 forward = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _characterLayer);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(forward, dirToTarget) < _viewPOV /2)
            {
                Debug.DrawLine(transform.position, target.position);
                _visibleTarget = target;
            }
        }
    }
    void ShootTarget()
    {
        if (_visibleTarget == null)
            return;

        if (_timer > _timeBetweenShots || !_firstShot)
        {
            SpawnProjectiles(_shootingOrigin.position, _visibleTarget.position);
            _firstShot = true;
            _timer = 0f;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    void SpawnProjectiles(Vector2 origin, Vector2 target)
    {
        BulletObject bulletObj = _bulletPool.Pop(_shootingOrigin.position);

        bulletObj.rb2D.velocity = (target - origin).normalized * _projectileSpeed;
    }
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
        Vector2 forward = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Vector2 endPoint = transform.position + (Quaternion.Euler(0, 0, _viewPOV * 0.5f) * forward);

        Vector2 position = (endPoint - (Vector2)transform.position).normalized;

        Handles.color = new Color(0f, 1f, 0f, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, position, _viewPOV, _viewRadius);
    }
#endif
}
