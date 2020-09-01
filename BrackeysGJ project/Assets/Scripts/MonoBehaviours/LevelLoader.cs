using System;
using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.ClassFiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string gameScene;
    [SerializeField] private GameObject menuAudio;
    private void Start()
    {
        if(GameObject.FindGameObjectsWithTag("AudioManager").Length > 0) return;
        var target = Instantiate(menuAudio);
        DontDestroyOnLoad(target);
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    
    
    //He did
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }
    
    public void LoadGame()
    {
        DoggoController.win = false;
        Destroy(GameObject.FindGameObjectWithTag("AudioManager"));
        SceneManager.LoadScene(gameScene);
    }
    
    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void LoadControlsScene()
    {
        SceneManager.LoadScene("ControlsScene");
    }

    public void ExitGame()
    {
        //Checks if the game in editor or a build
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;    
        #else
                Application.Quit();
        #endif
    }
}
