using UnityEngine;

[CreateAssetMenu(fileName ="New Character Stats", menuName ="Character/Stats")]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Horizontal")]
    [Tooltip("Max velocity that object can reach")]
    [SerializeField] float _maxSpeed = default;

    [Tooltip("Rate of change of velocity")]
    [SerializeField] float _maxAcceleration = default;

    [Header("Vertical")]
    [Tooltip("Initial vertical force")]
    [SerializeField] float _jumpHeight = default;

    [Tooltip("Initial vertical force")]
    [SerializeField] float _jumpingAcceleration = default;

    [Tooltip("Negative force which pulls character to the ground")]
    [Range(-50f, -1f)]
    [SerializeField] float _maxFallingForce = default;

    [Tooltip("Rate of change of falling force")]
    [SerializeField] float _fallingAcceleration = default;

    [Tooltip("Additional force when doing double jump")]
    [SerializeField] float _doubleJumpHeight = default;

    public float MaxSpeed { get { return _maxSpeed; } }
    public float MaxAcceleration { get { return _maxAcceleration; } }
    public float JumpHeight { get { return _jumpHeight; } }
    public float JumpingAcceleration { get { return _jumpingAcceleration; } }
    public float MaxFallingForce { get { return _maxFallingForce; } }
    public float FallingAcceleration { get { return _fallingAcceleration; } }
    public float DoubleJumpHeight { get { return _doubleJumpHeight; } }
}
