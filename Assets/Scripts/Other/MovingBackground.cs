using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MovingBackground : MonoBehaviour
    {
        SpriteRenderer _render;
        float _distance = 0f;

        [SerializeField] float _speed = 0.5f;
        Vector2 _origin;
        void Awake()
        {
            _render = GetComponent<SpriteRenderer>();

            _distance = _render.size.y * 0.5f;
            _origin = transform.position;
        }

        void Update()
        {
            if(Mathf.Abs(transform.position.y - _origin.y) > _distance)
            {
                transform.position = _origin;
            }
            else
            {
                transform.position = new Vector2(_origin.x, transform.position.y - _speed * Time.deltaTime);
            }
        }
    }
}