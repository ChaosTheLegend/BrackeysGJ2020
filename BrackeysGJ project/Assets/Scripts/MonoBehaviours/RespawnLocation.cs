﻿using System;
using BrackeysGJ.ClassFiles;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrackeysGJ.MonoBehaviours
{
    public class RespawnLocation : MonoBehaviour
    {
        [SerializeField] private Transform cam;
        [SerializeField] private string mainSceneName;
        private void Start()
        {
            var save = SaveSystem.Load();
            if(save == null) return;
            transform.position = save.GetPlayerPosition();
            cam.position = save.GetCameraPosition();
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(mainSceneName);
            if(Input.GetKeyDown(KeyCode.F1)) SaveSystem.DeleteSave();
        }
        #endif
    }
}
