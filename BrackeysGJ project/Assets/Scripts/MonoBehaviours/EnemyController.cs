using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectilePosition;
    private GameObject player;
    private Animator animator;
    
    // Configurations
    [SerializeField] private float health = 100f;  
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float delayDeathTime = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float waitTime = 1f;
    private bool fire = true;
    private float speedHolder;
    private float waitTimeHolder;
    private float distanceBetweenPlayerAndEnemy;

    // Animation States
    enum EnemyState
    {
        shooting,
        running,
        death
    };
    private EnemyState state;
    
    // Colliders
    private Collider2D boxCollider; // Never used but Collider helps in in-game Collision
    private Collider2D capsuleCollider; 

    // Rigibody
    private Rigidbody2D rb;
    
    void Start()
    {
        state = EnemyState.running;
        player = GameObject.FindGameObjectWithTag("Player");
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        waitTimeHolder = waitTime;
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
    }

    // For spawning shurikens
    void Update()
    {
        if (distanceBetweenPlayerAndEnemy < maxDistance && fire)
        {
            GameObject throwable = Instantiate(projectilePrefab, projectilePosition.transform.position, transform.rotation);
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
        
        // To Switch animation based on enemy state 
        SwitchAnimations();
    }
    
    // For enemy movements
    void FixedUpdate()
    {
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
        if (distanceBetweenPlayerAndEnemy < maxDistance)
        {
            // Get the Player Direction
            float x = player.transform.position.x - transform.position.x;
            FlipSpriteOnPlayerSight(x);
            state = EnemyState.shooting;
            return;
        }
        
        // Check if enemy is still touching the ground
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
        {
            rb.velocity = new Vector2(-movementSpeed, 0f);
            state = EnemyState.shooting;
            return;
        }

        state = EnemyState.running;
        rb.velocity = new Vector2(movementSpeed, 0f);
    }

    // For checking if enemy has come to the edge of the ground
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
    // public float GetDirection()
    // {
    //     return transform.localScale.x;
    // }

    // For taking Damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator EnemyDeath()
    {
        state = EnemyState.death;
        yield return new WaitForSeconds(delayDeathTime);
        Destroy(gameObject);
    }

    void SwitchAnimations()
    {
        if (state == EnemyState.shooting)
        {
            animator.SetBool("Shooting", true);
        }
        else if (state == EnemyState.running)
        {
            animator.SetBool("Shooting", false);
        }
        else
        {
            animator.SetTrigger("Death");
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
