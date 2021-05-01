using UnityEngine;

[CreateAssetMenu(fileName ="New Character Stats", menuName ="Character/Stats")]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("Max velocity that object can reach")]
    [SerializeField] float _maxSpeed;

    [Tooltip("Rate of change of velocity")]
    [SerializeField] float _maxAcceleration;

    [Tooltip("Initial vertical force")]
    [SerializeField] float _jumpForce;

    public float MaxSpeed { get { return _maxSpeed; } }
    public float MaxAcceleration { get { return _maxAcceleration; } }
    public float JumpForce { get { return _jumpForce; } }
}
