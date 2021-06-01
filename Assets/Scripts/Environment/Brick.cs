using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Brick : MonoBehaviour
{
    const float BOUNCE_DISTANCE = 1f;
    [SerializeField] bool _canSpawn = false;
    [SerializeField] float _bounceSpeed = 7f;
    [SerializeField] CoinPool _coinPool = default;
    [SerializeField] int _noOfCoin = default;
    [SerializeField] Sprite _disabledSprite = default;

    bool _canBounce = false;
    bool _disable = false;
    int _direction = 1; // 1: upward, -1: downward
    float _interpolation;

    Vector2 _origin;
    float _targetPositionY;
    int _coinRemain;

    SpriteRenderer _renderer;
    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _origin = transform.position;
        _targetPositionY = _origin.y + BOUNCE_DISTANCE;
        _coinRemain = _noOfCoin;
    }
    void Update()
    {
        Bounce();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_disable)
            return;

        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);

        // Got hit from bottom
        if (contacts[0].point.y < transform.position.y)
        {
            _canBounce = true;
            SpawnPrefab();
        }
    }
    void Bounce()
    {
        if (_disable)
            return;

        if (_canBounce)
        {
            _interpolation += _direction * _bounceSpeed * Time.deltaTime;
            _interpolation = Mathf.Clamp01(_interpolation);

            float positionY = Mathf.Lerp(_origin.y, _targetPositionY, _interpolation);

            transform.position = new Vector2(_origin.x, positionY);

            if(Mathf.Approximately(transform.position.y, _targetPositionY))
            {
                _direction *= -1;
            }

            if(Mathf.Approximately(transform.position.y, _origin.y) && _direction < 0)
            {
                _direction = 1;
                _canBounce = false;

                if (_coinRemain <= 0)
                {
                    Disable();
                }
            }
        }
    }
    void SpawnPrefab()
    {
        if (_canSpawn && _coinRemain > 0)
        {
            _coinPool.Pop(transform.position);
            _coinRemain--;
        }
    }
    void Disable()
    {
        if (!_canSpawn)
            return;
        _renderer.sprite = _disabledSprite;
        _disable = true;
    }
}
