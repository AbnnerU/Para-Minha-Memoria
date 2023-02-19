
using UnityEngine;

public class AnimationAction : InteractionAction
{
    [SerializeField] private Animator anim;

    [SerializeField] private string animationName;

    public override void ExecuteAction()
    {
        anim.Play(animationName, 0, 0);
    }
}
