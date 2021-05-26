using UnityEngine;

public class Brick : MonoBehaviour
{
    const float BOUNCE_DISTANCE = 1f;
    [SerializeField] float _bounceSpeed = 7f;
    [SerializeField] CoinPool _pool = default;
    [SerializeField] bool _canSpawn = false;

    bool _canBounce = false;
    int _direction = 1; // 1: upward, -1: downward
    float _interpolation;

    Vector2 _origin;
    float _targetPositionY;
    void Awake()
    {
        _origin = transform.position;
        _targetPositionY = _origin.y + BOUNCE_DISTANCE;
    }
    void Update()
    {
        Bounce();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);

        // Got hit from bottom
        if (contacts[0].point.y < transform.position.y)
        {
            _canBounce = true;

            if (_canSpawn)
            {
                CoinObject coin = _pool.Pop(transform.position);
            }
        }
    }
    void Bounce()
    {
        if(_canBounce)
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
            }
        }
    }
}
