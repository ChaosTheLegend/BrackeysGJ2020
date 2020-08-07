using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stuckInWallSounds;
    [SerializeField] private List<AudioClip> bounceWallSounds;
    [SerializeField] private List<AudioClip> rewindSounds;

    [SerializeField] private AudioSource srcOneShot;
    [SerializeField] private AudioSource srcLoop;

    [Header("Sound Intervals")]
    [SerializeField] private float rewindIntervalSeconds = 0.2f;

    private float rewindTimer;

    private void Start()
    {
        rewindTimer = 0f;
    }

    private void Update()
    {
        TimerCountdown();
    }

    internal void PlayStuck()
    {
        srcOneShot.PlayOneShot(GetRandomSound(stuckInWallSounds));
    }

    internal void PlayBounce()
    {
        srcOneShot.PlayOneShot(GetRandomSound(bounceWallSounds));
    }

    internal void PlayRewindLoop()
    {
        if(!srcLoop.isPlaying &&
            rewindTimer <= 0)
        {
            rewindTimer = rewindIntervalSeconds;

            srcLoop.clip = GetRandomSound(rewindSounds);
            srcLoop.Play();
        }
    }

    internal void StopRewindLoop()
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
        rewindTimer -= rewindTimer <= 0 ? 0 : Time.deltaTime;
    }

}
