using UnityEngine;

[CreateAssetMenu(fileName ="New Int Animation Parameter", menuName ="Character/Animation/Int Parameter")]
public class AnimationIntSO : ScriptableObject
{
    public string paramter;
    int _paramterHash;

    void OnEnable()
    {
        _paramterHash = Animator.StringToHash(paramter);
    }
    public void SetValue(Animator animator, int value)
    {
        animator.SetInteger(_paramterHash, value);
    }
}
