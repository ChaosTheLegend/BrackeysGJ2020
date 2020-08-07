using BrackeysGJ.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> shootSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> runSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> jumpSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> doubleJumpSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ladderClimbSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> hitSounds = new List<AudioClip>();

    [SerializeField] private AudioClip wallSlideSound;
    [SerializeField] private AudioClip dyingSound;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource srcLoop;
    [SerializeField] private AudioSource srcOneShot;

    [Header("Sound Intervals")]
    [SerializeField] private float runIntervalSeconds = 0.2f;
    [SerializeField] private float ladderClimbIntervalSeconds = 0.15f;
    [SerializeField] private float wallSlideIntervalSeconds = 0.2f;

    private float runTimer;
    private float ladderClimbTimer;
    private float wallSlideTimer;

    private void Start()
    {
        runTimer = 0f;
        ladderClimbTimer = 0f;
        wallSlideTimer = 0f;
    }

    private void Update()
    {
        TimerCountdown();
    }

    internal void PlayWallSlide()
    {
        if (!srcLoop.isPlaying &&
            wallSlideTimer <= 0)
        {
            wallSlideTimer = wallSlideIntervalSeconds;

            srcLoop.clip = wallSlideSound;
            srcLoop.Play();
        }

    }

    internal void PlayClimbing()
    {
        if (!srcLoop.isPlaying &&
            ladderClimbTimer <= 0)
        {
            ladderClimbTimer = ladderClimbIntervalSeconds;

            srcLoop.clip = GetRandomSound(ladderClimbSounds);
            srcLoop.Play();
        }
    }

    internal void PlayRunning()
    {
        if (!srcLoop.isPlaying &&
            runTimer <= 0)
        {
            runTimer = runIntervalSeconds;

            srcLoop.clip = GetRandomSound(runSounds);
            srcLoop.Play();
        }
    }

    internal void PlayHit()
    {
        srcOneShot.PlayOneShot(GetRandomSound(hitSounds));
    }

    internal void PlayJumping()
    {
        srcOneShot.PlayOneShot(GetRandomSound(jumpSounds));
    }

    internal void PlayDoubleJumping()
    {
        srcOneShot.PlayOneShot(GetRandomSound(doubleJumpSounds));
    }


    internal void PlayDying()
    {
        srcOneShot.PlayOneShot(dyingSound);
    }

    internal void PlayShoot()
    {
        srcOneShot.PlayOneShot(GetRandomSound(shootSounds));
    }

    /// <summary>
    /// Stops sounds from this game object that are not one shot (like running, sliding, or climbing)
    /// </summary>
    internal void StopSounds()
    {
        srcLoop.Stop();
    }

    /// <summary>
    /// Returns a random sound from the soundList.
    /// </summary>
    /// <param name="soundList">A list of AudioClips to choose from.</param>
    /// <returns></returns>
    private AudioClip GetRandomSound(List<AudioClip> soundList)
    {
        int index = UnityEngine.Random.Range(0, soundList.Count - 1);

        return soundList[index];
    }

    /// <summary>
    /// Countdown all timers until they reach 0 seconds.
    /// </summary>
    private void TimerCountdown()
    {
        // Countdown all timers until they reach 0
        runTimer -= runTimer <= 0 ? 0 : Time.deltaTime; ;
        ladderClimbTimer -= ladderClimbTimer <= 0 ? 0 : Time.deltaTime; ;
        wallSlideTimer -= wallSlideTimer <= 0 ? 0 : Time.deltaTime; ;
    }
}
