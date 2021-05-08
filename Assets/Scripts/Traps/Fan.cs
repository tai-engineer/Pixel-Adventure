using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] Animator _animator = default;
    [SerializeField] float _delayTime = default;
    [SerializeField] float _force = default;
    [SerializeField] bool _activate = true;

    int _parameterHash = Animator.StringToHash("On_Off");

    bool _startPushing = false;
    void Awake()
    {
        if(_animator == null)
        {
            Debug.LogError($"Missing Animator component in {transform.parent.name} game object");
            return;
        }

        _animator.SetBool(_parameterHash, _activate);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activate)
        {
            if (!_startPushing)
            {
                if (collision.TryGetComponent<CharacterController>(out CharacterController character))
                {
                    StartCoroutine(StartPushing(character));
                    _startPushing = true;
                }
                else
                {
                    Debug.LogWarning($"Not found CharacterController component in {collision.gameObject.name}");
                }

            } 
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _startPushing = false;
    }

    IEnumerator StartPushing(CharacterController character)
    {
        yield return new WaitForSeconds(_delayTime);

        character.SetJumpHeight(_force);
    }
}
