using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private AudioSource srcOneShot;
    [SerializeField] private AudioSource srcLoop;

    private void Start()
    {
        srcLoop.clip = runSound;
    }

    internal void PlayRunSoundLoop()
    {
        if(!srcLoop.isPlaying)
        {
            srcLoop.Play();
        }
    }

    internal void StopRunSoundLoop()
    {
        srcLoop.Stop();
    }

    internal void PlayShootSound()
    {
        srcOneShot.PlayOneShot(shootSound);
    }
    internal void PlayHitSound()
    {
        srcOneShot.PlayOneShot(hitSound);
    }

    internal void PlayDeathSound()
    {
        srcOneShot.PlayOneShot(deathSound);
    }
}
