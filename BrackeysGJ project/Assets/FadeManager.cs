using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    public UnityEvent onFadeIn;
    public UnityEvent onFadeOut;
    [SerializeField] private float fadeInDelay;
    [SerializeField] private float fadeOutDelay;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        Invoke($"TrueFadeIn",fadeInDelay);
    }

    private void TrueFadeIn()
    {
        _anim.SetTrigger("FadeIn");
    }
    
    private void TrueFadeOut()
    {
        _anim.SetTrigger("FadeOut");
    }

    public void FadeOut()
    {
        Invoke($"TrueFadeOut",fadeOutDelay);
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
