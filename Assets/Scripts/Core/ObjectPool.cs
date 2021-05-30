using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ObjectPool<TPool, TObject, TInfo> : ObjectPool<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject, TInfo>
    where TObject : PoolObject<TPool, TObject, TInfo>, new()
{
    void Start()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            TObject obj = CreateNewPoolObject();
            pool.Add(obj);
        }
    }
    public TObject Pop(TInfo info)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].inPool)
            {
                pool[i].inPool = false;
                pool[i].WakeUp(info);
                return pool[i];
            }
        }

        if (_extendable)
        {
            // All of pool objects are in used.
            // Create one more object to use
            TObject poolObj = CreateNewPoolObject();
            poolObj.inPool = false;
            poolObj.WakeUp(info);
            pool.Add(poolObj);
            return poolObj;
        }
        return null;
    }
}
public abstract class ObjectPool<TPool, TObject> : MonoBehaviour
    where TPool: ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public GameObject prefab;
    public int initialPoolCount = 10;
    public List<TObject> pool = new List<TObject>();
    public bool _extendable = true;

    void Start()
    {
        for(int i = 0; i < initialPoolCount; i++)
        {
            TObject obj = CreateNewPoolObject();
            pool.Add(obj);
        }
    }
    protected TObject CreateNewPoolObject()
    {
        TObject poolObject = new TObject();
        poolObject.instance = Instantiate(prefab, transform);
        poolObject.instance.name = prefab.name;
        poolObject.inPool = true;
        poolObject.SetReferences(this as TPool);
        poolObject.Sleep();
        return poolObject;
    }

    public TObject Pop()
    {
        for(int i = 0; i < pool.Count; i++)
        {
            if(pool[i].inPool)
            {
                pool[i].inPool = false;
                pool[i].WakeUp();
                return pool[i];
            }
        }

        if (_extendable)
        {
            // All of pool objects are in used.
            // Create one more object to use
            TObject poolObj = CreateNewPoolObject();
            poolObj.inPool = false;
            poolObj.WakeUp();
            pool.Add(poolObj);
            return poolObj; 
        }

        return default(TObject);
    }

    public virtual void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }
}

[Serializable]
public abstract class PoolObject<TPool, TObject, TInfo> : PoolObject<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject, TInfo>
    where TObject : PoolObject<TPool, TObject, TInfo>, new()
{
    public virtual void WakeUp(TInfo info) { }
    public override string ToString()
    {
        return $"{instance.name}: inPool({inPool})";
    }
}
[Serializable]
public abstract class PoolObject<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public bool inPool;
    public GameObject instance;
    public TPool objectPool;

    public void SetReferences(TPool pool)
    {
        objectPool = pool;
        SetReferences();
    }
    public virtual void WakeUp() { }
    
    public virtual void Sleep()
    {
        instance.SetActive(false);
    }
    public virtual void ReturnToPool()
    {
        TObject thisObject = this as TObject;
        objectPool.Push(thisObject);
    }
    protected virtual void SetReferences() { }
}
