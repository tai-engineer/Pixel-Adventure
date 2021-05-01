using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] InputReaderSO _input;
    [SerializeField] CharacterStatsSO _stats;
    [SerializeField] float _gravity;
    public bool JumpInput { get; private set; } = false;
    [NonSerialized] public Vector2 moveInput;
    [NonSerialized] public Vector2 moveVector;

    public CharacterStatsSO Stats { get { return _stats; } }
    public float Gravity { get { return _gravity; } }

    Rigidbody2D _rb;
    BoxCollider2D _box;
    void Awake()
    {
        _input.moveEvent            += OnMove;
        _input.jumpStartedEvent     += OnJumpStarted;
        _input.jumpCanceledEvent    += OnJumpCanceled;

        _rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        Move(moveVector);
    }
    void Move(Vector2 movement)
    {
        moveVector = movement;
        _rb.MovePosition(_rb.position + moveVector * Time.fixedDeltaTime);
    }
    void OnDisable()
    {
        _input.moveEvent            -= OnMove;
        _input.jumpStartedEvent     -= OnJumpStarted;
        _input.jumpCanceledEvent    -= OnJumpCanceled;
    }
    void OnMove(Vector2 input)
    {
        moveInput = input;
    }

    void OnJumpStarted()
    {
        JumpInput = true;
    }

    void OnJumpCanceled()
    {
        JumpInput = false;
    }
}
