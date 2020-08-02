using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject shuriken;
    [SerializeField] private GameObject shurikenPosition;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float minTime = 1f;
    [SerializeField] private float maxTime = 3f;
    private float spawnTime;
    
    private Collider2D boxCollider;
    private Collider2D capsuleCollider; // Never used but helps in in-game OnTriggerExit2D

    private Rigidbody2D rb;
    
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spawnTime = UnityEngine.Random.Range(minTime, maxTime);
    }

    // For spawning shurikens
    void Update()
    {
        if(spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else if (spawnTime <= 0)
        {
            GameObject throwable = Instantiate(shuriken, shurikenPosition.transform.position, transform.rotation);
            spawnTime = UnityEngine.Random.Range(minTime, maxTime);
        }
    }
    
    // For player movements
    void FixedUpdate()
    {
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        
        rb.velocity = new Vector2(movementSpeed, 0f);
    }

    // For checking if player has come to the edge of the ground
    private void OnTriggerExit2D(Collider2D other)
    {
        movementSpeed = -movementSpeed;
        FlipSprite();
    }

    // To change the player direction
    void FlipSprite()
    {
        Vector2 newScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        transform.localScale = newScale;
    }

    // Used to get the direction the shuriken must be thrown
    public float GetDirection()
    {
        return transform.localScale.x;
    } 
}
