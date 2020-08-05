using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    private LastCheckpointLocation lCL;
    
    void Start()
    {
        lCL = GameObject.FindGameObjectWithTag("Checkpoint Holder").GetComponent<LastCheckpointLocation>();
        transform.position = lCL.GetLastCheckpointPosition();
    }
}
