using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneAutoLoader : MonoBehaviour
{
    public bool edit;
    public string toLoad;
    public Transform cameraPos;
    public Bounds LoadBounds;
    public Bounds UnLoadBounds;
    public Bounds[] SceneBounds;
    public string[] SceneNames;
    private bool[] sceneLoaded;
    private void Start()
    {
        sceneLoaded = new bool[SceneBounds.Length];
        for(int i =0;i<SceneBounds.Length;i++)
        {     
            if (LoadBounds.Intersects(SceneBounds[i]))
            {
                if(!sceneLoaded[i]) LoadRuntime(i);
            }
        }
    }

    private void LoadRuntime(int id)
    {
        SceneManager.LoadScene(SceneNames[id], LoadSceneMode.Additive);
        sceneLoaded[id] = true;
    }
    public void AddScene(int id)
    {
        sceneLoaded[id] = true;
        StartCoroutine(BackgroundLoader(id));
    }
    // Start is called before the first frame update
    private IEnumerator BackgroundLoader(int id) {
        AsyncOperation load = SceneManager.LoadSceneAsync(SceneNames[id], LoadSceneMode.Additive);
        while (!load.isDone)
        {
            yield return null;
        }
    }
    private void loadCall()
    {
        for (int i = 0; i < SceneBounds.Length; i++)
        {
            if (LoadBounds.Intersects(SceneBounds[i]) && !sceneLoaded[i]) AddScene(i);
            else if (!LoadBounds.Intersects(SceneBounds[i]) && !UnLoadBounds.Intersects(SceneBounds[i]) && sceneLoaded[i]) RemoveScene(i);
        }
    }


    public void RemoveScene(int id)
    {
        sceneLoaded[id] = false;
        StartCoroutine(BackgroundUnloader(id));
    }
    private IEnumerator BackgroundUnloader(int id)
    {
        AsyncOperation load = SceneManager.UnloadSceneAsync(SceneNames[id]);
        while (!load.isDone)
        {
            yield return null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LoadBounds.center = new Vector3(cameraPos.position.x, cameraPos.position.y,0f);
        UnLoadBounds.center = LoadBounds.center;
    }

    private void LateUpdate()
    {
        loadCall();    
    }

    void onSceneGUI() { 
    
    }
    private void OnDrawGizmosSelected()
    {
        foreach (Bounds b in SceneBounds){
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(b.center, b.size);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(LoadBounds.center, LoadBounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(UnLoadBounds.center, UnLoadBounds.size);
    }

}
