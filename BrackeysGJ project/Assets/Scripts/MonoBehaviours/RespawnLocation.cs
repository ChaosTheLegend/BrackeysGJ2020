using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    private LastCheckpointLocation lCL;
    
    void Start()
    {
        lCL = GameObject.FindObjectOfType<LastCheckpointLocation>();
        
        // Spawn the player at the last checkpoint Location 
        transform.position = lCL.LastCheckpointPosition;
    }
}
