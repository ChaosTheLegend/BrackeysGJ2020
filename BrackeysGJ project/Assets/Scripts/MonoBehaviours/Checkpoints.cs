using System;
using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.ClassFiles;
using BrackeysGJ.Serializable;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private Transform _player;
    private Transform _camera;
    private bool _active = false;
    private Animator _anim;
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _camera = Camera.main.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Update current checkPoint location;
        if (!other.CompareTag("Player")) return;
        _anim.SetBool($"Active",true);
        if(!_active) _player.GetComponent<PlayerHealth>().GiveHealth(10000f);
        _active = true;
        var save = new SaveData(transform.position,_camera.position);
        SaveSystem.Save(save);
    }
}
