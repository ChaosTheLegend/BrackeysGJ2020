using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCheckpointLocation : MonoBehaviour
{
    // Should be created only once per level;
    private static LastCheckpointLocation instance;
    
    // Holds the last checkpoint location;
    public Vector2 LastCheckpointPosition;
    void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
    }
}
