using UnityEngine;

[CreateAssetMenu(fileName ="New Float Animation Parameter", menuName ="Character/Animation/Float Parameter")]
public class AnimationFloatSO : ScriptableObject
{
    public string paramter;
    int _paramterHash;

    void OnEnable()
    {
        _paramterHash = Animator.StringToHash(paramter);
    }
    public void SetValue(Animator animator, float value)
    {
        animator.SetFloat(_paramterHash, value);
    }
}
