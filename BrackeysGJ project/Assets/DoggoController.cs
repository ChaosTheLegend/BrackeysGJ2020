using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoController : MonoBehaviour
{
    private Animator _anim;
    public static bool win;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _anim.SetTrigger("Happy");
        }
    }

    private void setwin()
    {
        win = true;
    }
}
