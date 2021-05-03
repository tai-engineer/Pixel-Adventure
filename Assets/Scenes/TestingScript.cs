using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] InputReaderSO _input = default;

    void OnEnable()
    {
        _input.moveEvent            += OnMove;
        _input.jumpStartedEvent     += OnJumpStarted;
        _input.jumpCanceledEvent    += OnJumpCanceled;
    }

    void OnDisable()
    {
        _input.moveEvent            -= OnMove;
        _input.jumpStartedEvent     -= OnJumpStarted;
        _input.jumpCanceledEvent    -= OnJumpCanceled;
    }

    void OnMove(Vector2 movement)
    {
        Debug.Log("movement = " + movement);
    }

    void OnJumpStarted()
    {
        Debug.Log("Jump started");
    }

    void OnJumpCanceled()
    {
        Debug.Log("Jump canceled");
    }
}
