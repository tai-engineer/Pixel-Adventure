using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelAdventure
{
    /// <summary>
    /// Broadcast events for user inputs
    /// </summary>
    [CreateAssetMenu(fileName = "InputReader", menuName ="Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IPlayerControlsActions
    {
        // Assign delegate{} to events to initialise them with an empty delegate
        // so we can skip the null check when we use them

        // Gameplay
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction moveCanceledEvent = delegate { };
        public event UnityAction jumpEvent = delegate { };
        public event UnityAction jumpCanceledEvent = delegate { };

        GameInput _gameInput;

        void OnEnable()
        {
            if(_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.PlayerControls.SetCallbacks(this);
            }

            EnablePlayerControlsInput();
        }

        void OnDisable()
        {
            DisableAllInputs();
        }
        void EnablePlayerControlsInput()
        {
            _gameInput.PlayerControls.Enable();
        }
        void DisableAllInputs()
        {
            _gameInput.PlayerControls.Disable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                moveEvent.Invoke(context.ReadValue<Vector2>());

            if (context.phase == InputActionPhase.Canceled)
                moveCanceledEvent.Invoke();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                jumpEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                jumpCanceledEvent.Invoke();
        }
    }
}