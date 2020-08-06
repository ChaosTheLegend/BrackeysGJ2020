using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
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

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("OptionsScene");
    }
}
