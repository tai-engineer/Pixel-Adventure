using UnityEngine;

public class BulletPool : ObjectPool<BulletPool, BulletObject>
{
    
}

public class BulletObject : PoolObject<BulletPool, BulletObject>
{
    public Transform transform;
    public Rigidbody2D rb2D;
    public Bullet bullet;

    protected override void SetReferences()
    {
        transform = instance.transform;
        rb2D = instance.GetComponent<Rigidbody2D>();
        bullet = instance.GetComponent<Bullet>();
        bullet.bulletPoolObject = this;
        bullet.mainCamera = Object.FindObjectOfType<Camera>();
    }

    public override void WakeUp(Vector2 position)
    {
        transform.position = position;
        instance.SetActive(true);
    }
}
