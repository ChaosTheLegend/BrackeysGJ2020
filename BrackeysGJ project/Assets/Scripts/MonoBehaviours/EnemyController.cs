using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject shuriken;
    [SerializeField] private GameObject shurikenPosition;
    private GameObject player;
    
    // Configurations
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float waitTime = 3f;
    private bool fire = true;
    private float distanceHolder;
    private float speedHolder;
    private float distanceBetweenPlayerAndEnemy;
    
    // Colliders
    private Collider2D boxCollider;
    private Collider2D capsuleCollider; // Never used but Collider helps in in-game OnTriggerExit2D

    // Rigibody
    private Rigidbody2D rb;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        speedHolder = movementSpeed;
        distanceHolder = maxDistance;
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
    }

    // For spawning shurikens
    void Update()
    {
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
        
        if (distanceBetweenPlayerAndEnemy < maxDistance && fire)
        {
            GameObject throwable = Instantiate(shuriken, shurikenPosition.transform.position, transform.rotation);
            fire = false;
        }
        else
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                fire = true;
                waitTime = distanceHolder;
            }
        }
    }
    
    // For enemy movements
    void FixedUpdate()
    {
        if (distanceBetweenPlayerAndEnemy < maxDistance)
        {
            float x = player.transform.position.x - transform.position.x;
            FlipSpriteOnPlayerSight(x);
            return;
        }
        
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            rb.velocity = new Vector2(-movementSpeed, 0f);
            return;
        }

        rb.velocity = new Vector2(movementSpeed, 0f);
    }

    // For checking if player has come to the edge of the ground
    private void OnTriggerExit2D(Collider2D other)
    {
        FlipSpriteOnEdge();
    }

    // To change the enemy direction
    void FlipSpriteOnEdge()
    {
        movementSpeed = -movementSpeed;
        Vector2 newScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        transform.localScale = newScale;
    }

    void FlipSpriteOnPlayerSight(float x)
    {
        movementSpeed = Mathf.Sign(x) * speedHolder;
        Vector2 newScale = new Vector2(Mathf.Sign(x), transform.localScale.y);
        transform.localScale = newScale;
    }

    // Used to get the direction the shuriken must be thrown
    public float GetDirection()
    {
        return transform.localScale.x;
    }

    // private float GetPlayerPosition()
    // {
    //     if (Mathf.Abs(player.transform.position.y - transform.position.y) <= 2)
    //     {
    //         return Mathf.Abs(transform.position.x - player.transform.position.x);
    //     }
    //
    //     return maxDistance;
    // }
}
