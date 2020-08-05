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

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _camera = Camera.main.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Update current checkPoint location;
        if (!other.CompareTag("Player")) return;
        var save = new SaveData(_player.position,_camera.position);
        SaveSystem.Save(save);
    }
}
