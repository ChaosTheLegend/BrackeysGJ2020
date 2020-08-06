using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private LevelLoader levelLoader;
    [SerializeField] private float delayLoadTime = 3f;
    void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.health() <= 0)
        {
            StartCoroutine(LoadLevelAgain());
        }
    }

    IEnumerator LoadLevelAgain()
    {
        yield return new WaitForSeconds(delayLoadTime);
        levelLoader.LoadCurrentScene();
    }
}
