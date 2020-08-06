using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obsticals : MonoBehaviour
{
    private Collider2D obsticalCollider;
    private PlayerHealth playerHealth; 

    void Start()
    {
        obsticalCollider = GetComponent<Collider2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (obsticalCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            playerHealth.PlayerDied();
        }
    }
}
