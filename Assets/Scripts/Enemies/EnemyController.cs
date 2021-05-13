using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif
public class EnemyController : MonoBehaviour
{
    [SerializeField] LayerMask _characterLayer = default;
    [SerializeField] float _viewRadius = default;
    [Range(0f, 360f)]
    [SerializeField] float _viewPOV = default;
    [Range(0, 360)]
    [SerializeField] float _viewAngle = default;

    List<Transform> _visibleTargets = new List<Transform>();

    void Update()
    {
        ScanForPlayer();
    }
    void ScanForPlayer()
    {
        Vector2 forward = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _characterLayer);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(forward, dirToTarget) < _viewPOV /2)
            {
                _visibleTargets.Add(target);
                Debug.DrawLine(transform.position, target.position);
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        DrawConeView();
        DrawView();
    }
    void DrawView()
    {
        Handles.color = new Color(1.0f, 0f, 0f, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, _viewRadius);
    }
    void DrawConeView()
    {
        Vector2 forward = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0f, 0f, _viewAngle) * forward;

        Vector2 endPoint = transform.position + (Quaternion.Euler(0, 0, _viewPOV * 0.5f) * forward);

        Vector2 position = (endPoint - (Vector2)transform.position).normalized;

        Handles.color = new Color(0f, 1f, 0f, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, position, _viewPOV, _viewRadius);
    }
#endif
}
