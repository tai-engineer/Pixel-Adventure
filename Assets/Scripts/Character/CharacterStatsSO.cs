using UnityEngine;

[CreateAssetMenu(fileName ="New Character Stats", menuName ="Character/Stats")]
public class CharacterStatsSO : ScriptableObject
{
    #region Horizontal
    [Header("Horizontal")]
    [Tooltip("Max velocity that object can reach")]
    [SerializeField] float _maxSpeed = default;
    [Tooltip("Rate of change of velocity")]
    [SerializeField] float _maxAcceleration = default;
    [Tooltip("Proportion to simulate air resistance")]
    [SerializeField] float _airResistance = default;
    #endregion
    #region Vertical
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
    public float AirResistance { get { return _airResistance; } }
    public float JumpHeight { get { return _jumpHeight; } }
    public float JumpingAcceleration { get { return _jumpingAcceleration; } }
    public float MaxFallingForce { get { return _maxFallingForce; } }
    public float FallingAcceleration { get { return _fallingAcceleration; } }
    public float DoubleJumpHeight { get { return _doubleJumpHeight; } }
    #endregion
    #region Wall Slide
    [Header("Wall Slide")]
    [Tooltip("Speed when sliding on wall")]
    [SerializeField] float _wallSlideSpeed = default;
    [Tooltip("Distance when jumping from wall")]
    [SerializeField] float _wallJumpDistance = default;
    [Tooltip("Height when jumping from wall")]
    [SerializeField] float _wallJumpHeight = default;
    public float WallSlideSpeed { get { return _wallSlideSpeed; } }
    public float WallJumpDistance { get { return _wallJumpDistance; } }
    public float WallJumpHeight { get { return _wallJumpHeight; } }
    #endregion
    #region Health
    [Space]
    [Header("Health")]
    [SerializeField] float _health = 100f;

    public float Health { get { return _health;} }
    public void IncreaseHealth(float value)
    {
        _health += value;
    }
    public void DecreaseHealth(float value)
    {
        _health -= value;
    }
    #endregion
    #region Attack
    [Space]
    [Header("Attack")]
    [SerializeField] float _damage = default;

    public float Damage { get { return _damage; } }
    #endregion
    #region Hurt
    [Space]
    [Header("Hurt")]
    [Tooltip("Fall back distance when get hurt")]
    [SerializeField] float _fallBackDistance = default;
    [Tooltip("Fall back speed when get hurt")]
    [SerializeField] float _fallBackSpeed = default;

    public float FallBackDistance { get { return _fallBackDistance; } }
    public float FallBackSpeed { get { return _fallBackSpeed; } }
    #endregion
}
