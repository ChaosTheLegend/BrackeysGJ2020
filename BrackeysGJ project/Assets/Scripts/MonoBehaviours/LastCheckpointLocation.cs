using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCheckpointLocation : MonoBehaviour
{
    // Should be created only once per level;
    private static LastCheckpointLocation instance;
    
    // Holds the last checkpoint location;
    public Vector2 lastCheckpointPosition;
    void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
        
    }

    public void UpdateCheckpoint(Vector2 location)
    {
        lastCheckpointPosition = location;
    }

    public Vector2 GetCheckpoint()
    {
        return lastCheckpointPosition;
    }
}
