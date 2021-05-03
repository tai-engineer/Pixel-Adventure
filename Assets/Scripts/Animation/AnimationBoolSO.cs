using UnityEngine;

[CreateAssetMenu(fileName ="New Bool Animation Parameter", menuName ="Character/Animation/Bool Parameter")]
public class AnimationBoolSO : ScriptableObject
{
    public string paramter;
    int _paramterHash;

    void OnEnable()
    {
        _paramterHash = Animator.StringToHash(paramter);
    }

    public void SetValue(Animator animator, bool value)
    {
        animator.SetBool(_paramterHash, value);
    }
}
