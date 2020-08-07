using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private BackgorundMusicManager manager;

    [SerializeField] private AudioClip newClip;

    [SerializeField] private float fadeTime;

    [SerializeField] private bool oneShot;
    // Start is called before the first frame update

    public void ChangeMusic()
    {
        manager.ChangeBackgroundMusic(newClip,fadeTime);
        manager.setLoop(!oneShot);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ChangeMusic();
    }
}
