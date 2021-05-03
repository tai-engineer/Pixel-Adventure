using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    #region Animation Parameters
    [Header("Animation Parameters")]
    [SerializeField] AnimationBoolSO _runParameter = default;
    #endregion
    public AnimationBoolSO Run { get { return _runParameter; } }
}
