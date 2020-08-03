using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrow : MonoBehaviour
{
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode rewindButton = KeyCode.Mouse1;

    [SerializeField] private int maxShuriken = 3;

    // Number of shuriken that have been thrown
    private int shurikenCount = 0;

    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private GameObject shurikenSpawnPoint;
    [SerializeField] private GameObject shurikenSpawnParent;

    private Vector2 mousePos;
    private Vector2 mouseDirection;

    [SerializeField] private Shuriken[] shurikenInstances;
        
    // Start is called before the first frame update
    void Start()
    {
        shurikenInstances = new Shuriken[maxShuriken];
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(rewindButton) &&
            shurikenCount != 0)
        {
            RewindShuriken();
        }

        // Shuriken Throw check
        if (Input.GetKeyDown(shootButton) && 
            shurikenCount < maxShuriken)
        {
            ThrowShuriken();
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

    private void ThrowShuriken()
    {
        Shuriken shuriken = Instantiate(shurikenPrefab, shurikenSpawnPoint.transform.position, Quaternion.identity).GetComponent<Shuriken>();
        shurikenInstances[shurikenCount] = shuriken;

        shurikenCount++;
    }

    private void RewindShuriken()
    {
        foreach (Shuriken shuriken in shurikenInstances)
        {
            if (shuriken != null)
            {
                shuriken.Rewind();
            }
        }
    }

    public void ReturnShuriken()
    {
        // Remove one shuriken from the scene
        shurikenCount--;
    }

    public int GetAmmoLeft()
    {
        return maxShuriken - shurikenCount;
    }
}
