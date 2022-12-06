using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_PlayAnimation : BTBaseNode
{
    private readonly string animationName;
    private readonly Animator animator;
    
    public BT_PlayAnimation(Animator _anim, string _animationName)
    {
        animationName = _animationName;
        animator = _anim;
    }

    public override TaskStatus Run()
    {
        animator.Play(animationName);
        return TaskStatus.Success;
    }
}
