using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject _piecePrefab = default;
    [SerializeField] float _timeBeforeAutoDestruct = default;
    [SerializeField] bool _destroyWhenOutOfView = default;

    [HideInInspector] public BulletObject bulletPoolObject;
    [HideInInspector] public Camera mainCamera;

    float _damage;
    float _timer;

    const float k_OffScreenError = 0.01f;
    void Awake()
    {
        _timer = 0f;
    }
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    void Update()
    {
        if (_destroyWhenOutOfView)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > -k_OffScreenError &&
                screenPoint.x < 1f + k_OffScreenError && screenPoint.y > -k_OffScreenError &&
                screenPoint.y < 1f + k_OffScreenError;

            if (!onScreen)
            {
                bulletPoolObject.ReturnToPool();
            }
        }

        if (_timeBeforeAutoDestruct > 0)
        {
            _timer += Time.deltaTime;
            if(_timer > _timeBeforeAutoDestruct)
            {
                SelfDestruct();
                _timer = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        //TODO: Play impact sound
    }

    void SpawnPieces()
    {
        Instantiate(_piecePrefab, transform);
    }

    void SelfDestruct()
    {
        //SpawnPieces();
        bulletPoolObject.ReturnToPool();
    }
}
