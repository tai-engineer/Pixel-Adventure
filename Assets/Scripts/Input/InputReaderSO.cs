using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Input Reader", menuName ="Input/InputReader")]
public class InputReaderSO : ScriptableObject, InputControl.IGameplayActions
{
    InputControl _inputControl;

    public event UnityAction<Vector2> moveEvent = delegate { };
    public event UnityAction jumpStartedEvent = delegate { };
    public event UnityAction jumpCanceledEvent = delegate { };
    public event UnityAction shootEvent = delegate { };
    void OnEnable()
    {
        if(_inputControl == null)
        {
            _inputControl = new InputControl();
            _inputControl.Gameplay.SetCallbacks(this);
        }

        EnableGameplayControl();
    }

    void OnDisable()
    {
        DisableAllControls();
    }

    void EnableGameplayControl()
    {
        _inputControl.Gameplay.Enable();
    }

    void DisableAllControls()
    {
        _inputControl.Gameplay.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            jumpStartedEvent.Invoke();
        else if(context.phase == InputActionPhase.Canceled)
        {
            jumpCanceledEvent.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            shootEvent.Invoke();
    }
}
