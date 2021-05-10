using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IDamager
{
    [SerializeField] float _damage = default;
    public void Damage(GameObject obj, float damage)
    {
        IDamageable damageable = obj.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damage(collision.gameObject, _damage);
    }
}
