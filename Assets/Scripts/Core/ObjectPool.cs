using UnityEngine;
using System.Collections.Generic;
public class ObjectPool<TPool, TObject> : MonoBehaviour
    where TPool: ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public GameObject prefab;
    public int initialPoolCount = 10;
    List<TObject> pool = new List<TObject>();
    [SerializeField] bool _extendable = true;

    void Start()
    {
        for(int i = 0; i < initialPoolCount; i++)
        {
            TObject obj = CreateNewPoolObject();
            pool.Add(obj);
        }
    }
    TObject CreateNewPoolObject()
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

    public TObject Pop(Vector2 postion)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].inPool)
            {
                pool[i].inPool = false;
                pool[i].WakeUp(postion);
                return pool[i];
            }
        }

        if (_extendable)
        {
            // All of pool objects are in used.
            // Create one more object to use
            TObject poolObj = CreateNewPoolObject();
            poolObj.inPool = false;
            poolObj.WakeUp(postion);
            pool.Add(poolObj);
            return poolObj; 
        }

        return default(TObject);
    }

    public void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
        pool.Add(poolObject);
    }
}

public class PoolObject<TPool, TObject>
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
    public virtual void WakeUp(Vector2 postion) { }
    public void Sleep()
    {
        instance.SetActive(false);
    }
    public void ReturnToPool()
    {
        TObject thisObject = this as TObject;
        objectPool.Push(thisObject);
    }
    protected virtual void SetReferences() { }
}
