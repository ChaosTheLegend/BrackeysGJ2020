using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSound : MonoBehaviour
{
    [SerializeField] private AudioClip stuckInWallSound;
    [SerializeField] private AudioClip bounceWallSound;
    [SerializeField] private AudioClip rewindSound;

    [SerializeField] private AudioSource srcOneShot;
    [SerializeField] private AudioSource srcLoop;

    private void Start()
    {
        srcLoop.clip = rewindSound;
    }

    internal void PlayStuck()
    {
        srcOneShot.PlayOneShot(stuckInWallSound);
    }

    internal void PlayBounce()
    {
        srcOneShot.PlayOneShot(bounceWallSound);
    }

    internal void PlayRewindLoop()
    {
        if(!srcLoop.isPlaying)
        {
            srcLoop.Play();
        }
    }

    internal void StopRewindLoop()
    {
        srcLoop.Stop();
    }
}
