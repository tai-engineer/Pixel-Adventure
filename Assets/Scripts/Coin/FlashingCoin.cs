using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingCoin : MonoBehaviour
{
    [SerializeField] float _tossDistance = 3f;
    [SerializeField] float _tossSpeed = 5f;

    float _interpolation = 0f;
    int _direction = 1; // 1: upward, -1: downward

    Vector2 _origin;
    float _targetPositionY;
    public CoinObject coinPoolObject;

    void OnEnable()
    {
        _origin = transform.position;
        _targetPositionY = _origin.y + _tossDistance;
    }
    void Update()
    {
        TossUp();
    }
    void TossUp()
    {
        _interpolation += _direction * _tossSpeed * Time.deltaTime;
        _interpolation = Mathf.Clamp01(_interpolation);

        float positionY = Mathf.Lerp(_origin.y, _targetPositionY, _interpolation);

        transform.position = new Vector2(_origin.x, positionY);

        if (Mathf.Approximately(transform.position.y, _targetPositionY))
        {
            _direction *= -1;
        }

        if (Mathf.Approximately(transform.position.y, _origin.y) && _direction < 0)
        {
            _direction = 1;
            SelfDestroy();
        }
    }

    void SelfDestroy()
    {
        if(coinPoolObject != null)
        {
            coinPoolObject.ReturnToPool();
        }
    }
}
