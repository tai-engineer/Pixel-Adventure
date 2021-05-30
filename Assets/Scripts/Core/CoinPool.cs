using System.Collections.Generic;
using UnityEngine;

public class CoinPool : ObjectPool<CoinPool, CoinObject, Vector2>
{
}

public class CoinObject : PoolObject<CoinPool, CoinObject, Vector2>
{
    public Transform transform;
    public FlashingCoin coin;
    protected override void SetReferences()
    {
        transform = instance.transform;
        coin = instance.GetComponent<FlashingCoin>();
        coin.coinPoolObject = this;
    }

    public override void WakeUp(Vector2 postion)
    {
        transform.localPosition = postion;
        instance.SetActive(true);
    }
}
