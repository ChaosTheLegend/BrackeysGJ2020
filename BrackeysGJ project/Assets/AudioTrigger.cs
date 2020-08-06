using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private BackgorundMusicManager manager;

    [SerializeField] private AudioClip newClip;

    [SerializeField] private float fadeTime;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        manager.ChangeBackgroundMusic(newClip,fadeTime);
    }
}
