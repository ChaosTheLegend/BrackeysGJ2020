using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private LastCheckpointLocation lCL;
    
    void Start()
    {
        lCL = GameObject.FindObjectOfType<LastCheckpointLocation>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Update current checkPoint location;
        if (other.CompareTag("Player"))
        {
            lCL.LastCheckpointPosition = transform.position;
        }
    }
}
