using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShurikenThrow : MonoBehaviour
{
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode rewindButton = KeyCode.Mouse1;

    [SerializeField] private int maxShuriken = 3;


    public int MaxShuriken
    {
        get
        {
            return maxShuriken;
        }
    }

    // Number of shuriken that have been thrown
    private int shurikenCount = 0;

    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private GameObject shurikenSpawnPoint;
    [SerializeField] private GameObject shurikenSpawnParent;

    public UnityEvent onShoot;
    
    private Vector2 mousePos;
    private Vector2 mouseDirection;
    [HideInInspector]
    public bool dir;
    
    [SerializeField] private List<Shuriken> shurikenInstances;

    private PlayerHealth _health;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<PlayerHealth>();
        shurikenInstances = new List<Shuriken>();
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
        dir = mouseDirection.x >= 0;
        // Flip spawner to face x direction of mouse
        shurikenSpawnParent.transform.rotation = Quaternion.Euler(0, dir? 180 : 0, 0);

        #endregion
    }

    private void ThrowShuriken()
    {
        if(_health.CheckIfDead()) return;
        Shuriken shuriken = Instantiate(shurikenPrefab, shurikenSpawnPoint.transform.position, Quaternion.identity).GetComponent<Shuriken>();
        shurikenInstances.Add(shuriken);

        shurikenCount++;
        onShoot?.Invoke();
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

    /// <summary>
    /// Gives the player +1 shuriken(duh)
    /// </summary>
    public void ReturnShuriken(Shuriken shuriken)
    {
        // Remove one shuriken from the scene
        shurikenCount--;
        shurikenInstances.Remove(shuriken);
    }

    public int GetAmmoLeft()
    {
        return maxShuriken - shurikenCount;
    }
}
