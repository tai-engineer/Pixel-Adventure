using UnityEngine;

[CreateAssetMenu(fileName ="New Trigger Animation Parameter", menuName = "Character/Animation/Trigger Parameter")]
public class AnimationTriggerSO : ScriptableObject
{
    public string paramter;
    int _paramterHash;

    void OnEnable()
    {
        _paramterHash = Animator.StringToHash(paramter);
    }
    public void SetValue(Animator animator)
    {
        animator.SetTrigger(_paramterHash);
    }
}
