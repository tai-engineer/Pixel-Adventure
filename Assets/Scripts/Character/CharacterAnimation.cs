using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    #region Animation Parameters
    [Header("Animation Parameters")]
    [SerializeField] AnimationBoolSO _runParameter = default;
    [SerializeField] AnimationBoolSO _jumpParameter = default;
    [SerializeField] AnimationBoolSO _fallParameter = default;
    [SerializeField] AnimationBoolSO _doubleJumpParameter = default;
    #endregion
    public AnimationBoolSO Run { get { return _runParameter; } }
    public AnimationBoolSO Jump { get { return _jumpParameter; } }
    public AnimationBoolSO Fall { get { return _fallParameter; } }
    public AnimationBoolSO DoubleJump { get { return _doubleJumpParameter; } }
}
