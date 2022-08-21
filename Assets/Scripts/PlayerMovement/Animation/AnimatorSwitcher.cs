using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSwitcher : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SwitchAnimation(string key)
    {
        _animator.SetTrigger(key);
    }

    public void SetCountAnimator(string key, int count)
    {
        _animator.SetInteger(key, count);
    }
}
