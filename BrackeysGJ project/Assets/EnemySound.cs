using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> shootSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> runSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> hitSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> deathSounds = new List<AudioClip>();

    [SerializeField] private AudioSource srcOneShot;
    [SerializeField] private AudioSource srcLoop;

    [Header("Sound Intervals")]
    [SerializeField] private float runIntervalSeconds = 0.2f;

    private float runTimer;



    private void Start()
    {
        runTimer = 0f;
    }

    private void Update()
    {
        TimerCountdown();
    }

    internal void PlayRunSoundLoop()
    {
        if (!srcLoop.isPlaying &&
            runTimer <= 0)
        {
            runTimer = runIntervalSeconds;

            srcLoop.clip = GetRandomSound(runSounds);
            srcLoop.Play();
        }
    }

    internal void StopRunSoundLoop()
    {
        srcLoop.Stop();
    }

    internal void PlayShootSound()
    {
        srcOneShot.PlayOneShot(GetRandomSound(shootSounds));
    }
    internal void PlayHitSound()
    {
        srcOneShot.PlayOneShot(GetRandomSound(hitSounds));
    }

    internal void PlayDeathSound()
    {
        srcOneShot.PlayOneShot(GetRandomSound(deathSounds));
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
        runTimer -= runTimer <= 0 ? 0 : Time.deltaTime;
    }
}
