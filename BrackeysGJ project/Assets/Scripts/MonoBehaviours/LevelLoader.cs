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

    public void LoadSavedGame()
    {
        // Chaos will take of this hopefully :p
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
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
