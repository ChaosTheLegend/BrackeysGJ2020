using BrackeysGJ.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip wallSlideSound;
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip ladderClimbSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip dyingSound;


    [SerializeField] private AudioSource srcLoop;
    [SerializeField] private AudioSource srcOneShot;

    internal void PlayWallSlide()
    {
        srcLoop.clip = wallSlideSound;

        if (!srcLoop.isPlaying)
        {
            srcLoop.Play();
        }
    }

    /// <summary>
    /// Stops sounds from this game object that are not one shot (like running, sliding, or climbing)
    /// </summary>
    internal void StopSounds()
    {
        srcLoop.Stop();
    }

    internal void PlayClimbing()
    {
        srcLoop.clip = ladderClimbSound;

        if (!srcLoop.isPlaying)
        {
            srcLoop.Play();
        }
    }

    internal void PlayRunning()
    {
        srcLoop.clip = runSound;

        if (!srcLoop.isPlaying)
        {
            srcLoop.Play();
        }
    }

    internal void PlayHit()
    {
        srcOneShot.PlayOneShot(hitSound);
    }

    internal void PlayJumping()
    {
        srcOneShot.PlayOneShot(jumpSound);
    }

    internal void PlayDoubleJumping()
    {
        srcOneShot.PlayOneShot(doubleJumpSound);
    }


    internal void PlayDying()
    {
        srcOneShot.PlayOneShot(dyingSound);
    }

    internal void PlayShoot()
    {
        srcOneShot.PlayOneShot(shootSound);
    }
}
