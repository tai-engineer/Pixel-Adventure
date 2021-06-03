using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectilePool : ObjectPool<PlayerProjectilePool, PlayerProjectileObject, Vector2>
{
    
}

public class PlayerProjectileObject : PoolObject<PlayerProjectilePool, PlayerProjectileObject, Vector2>
{
    public Projectile projectile;

    protected override void SetReferences()
    {
        projectile = instance.GetComponent<Projectile>();
        projectile.projectileObject = this;
    }

    public override void WakeUp(Vector2 postion)
    {
        instance.transform.position = postion;
        instance.SetActive(true);
        projectile.PlaySoundFX();
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
    }
}
