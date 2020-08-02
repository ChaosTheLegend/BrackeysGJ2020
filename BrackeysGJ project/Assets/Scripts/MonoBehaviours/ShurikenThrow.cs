using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrow : MonoBehaviour
{
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;

    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private GameObject shurikenSpawnPoint;
    [SerializeField] private GameObject shurikenSpawnParent;

    private Vector2 mousePos;
    private Vector2 mouseDirection;

        
    // Start is called before the first frame update
    void Start()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        // Throw a shuriken
        if (Input.GetKeyDown(shootButton))
        {
            Instantiate(shurikenPrefab, shurikenSpawnPoint.transform.position, Quaternion.identity);
        }

        #region Spawner Flipping
        // Collect current mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the direction to the mouse
        mouseDirection = mousePos - new Vector2(transform.position.x, transform.position.y);

        // Flip spawner to face x direction of mouse
        if (mouseDirection.x >= 0)
        {
            shurikenSpawnParent.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            shurikenSpawnParent.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        #endregion
    }
}
