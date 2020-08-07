using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    public UnityEvent onFadeIn;
    public UnityEvent onFadeOut;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        _anim.SetTrigger("FadeIn");
    }
    
    
    public void FadeOut()
    {
        _anim.SetTrigger("FadeOut");
    }

    public void OnFadeOutFinish()
    {
        onFadeOut?.Invoke();
    }
    
    public void OnFadeInFinish()
    {
        onFadeIn?.Invoke();
    }
}
