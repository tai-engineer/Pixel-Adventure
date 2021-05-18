using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    Transform _cameraTransform;
    Vector3 _lastCameraPostion;

    [SerializeField] Vector2 _paralaxEffectMultiplier;
    [SerializeField] bool _infiniteX;
    [SerializeField] bool _infiniteY;
    Vector2 _textureUnitSize;
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPostion = _cameraTransform.position;

        Sprite sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSize.x = texture.width / sprite.pixelsPerUnit;
        _textureUnitSize.y = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaPosition = _cameraTransform.position - _lastCameraPostion;
        transform.position += new Vector3(deltaPosition.x * _paralaxEffectMultiplier.x, deltaPosition.y * _paralaxEffectMultiplier.y, 0f);
        _lastCameraPostion = _cameraTransform.position;

        RelocateByUnitSize(transform.position, _cameraTransform.position, _textureUnitSize);
    }

    void RelocateByUnitSize(Vector3 current, Vector3 target, Vector2 unitSize)
    {
        float distanceX = Mathf.Abs(target.x - current.x);
        float distanceY = Mathf.Abs(target.y - current.y);

        if (_infiniteX)
        {
            if (distanceX >= _textureUnitSize.x)
            {
                float offsetX = distanceX % _textureUnitSize.x;
                transform.position = new Vector3(target.x + offsetX, current.y);
            }
        }

        if (_infiniteY)
        {
            if (distanceY >= _textureUnitSize.y)
            {
                float offsetY = distanceY % _textureUnitSize.y;
                transform.position = new Vector3(transform.position.x, target.y + offsetY);
            }
        }
    }
}
