using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private GameObject shurikenPosition;
    private GameObject player;
    
    // Configurations
    [SerializeField] private float health = 100f;  
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float waitTime = 1f;
    private bool fire = true;
    private float speedHolder;
    private float waitTimeHolder;
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
        waitTimeHolder = waitTime;
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
    }

    // For spawning shurikens
    void Update()
    {
        if (distanceBetweenPlayerAndEnemy < maxDistance && fire)
        {
            GameObject throwable = Instantiate(shurikenPrefab, shurikenPosition.transform.position, transform.rotation);
            fire = false;
        }
        else
        {
            if (waitTime > 0 && !fire)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                fire = true;
                waitTime = waitTimeHolder;
            }
        }
    }
    
    // For enemy movements
    void FixedUpdate()
    {
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
        if (distanceBetweenPlayerAndEnemy < maxDistance)
        {
            float x = player.transform.position.x - transform.position.x;
            FlipSpriteOnPlayerSight(x);
            return;
        }
        
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
        {
            rb.velocity = new Vector2(-movementSpeed, 0f);
            return;
        }

        rb.velocity = new Vector2(movementSpeed, 0f);
    }

    // For checking if player has come to the edge of the ground
    private void OnTriggerExit2D(Collider2D other)
    {
        movementSpeed = -1 * movementSpeed;
        FlipSpriteOnEdge();
    }

    // To change the enemy direction
    void FlipSpriteOnEdge()
    {    
        transform.Rotate(0f,180f,0f);
    }

    void FlipSpriteOnPlayerSight(float x)
    {
        movementSpeed = Mathf.Sign(x) * Mathf.Abs(movementSpeed);
        Vector2 newScale = new Vector2(Mathf.Sign(x), transform.localScale.y);
        transform.localScale = newScale;
    }

    // Used to get the direction the shuriken must be thrown
    public float GetDirection()
    {
        return transform.localScale.x;
    }

    // For taking Damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
