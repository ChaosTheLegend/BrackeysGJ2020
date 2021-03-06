﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectilePosition;
    [SerializeField] private HealthBar healthBarPrefab;
    private GameObject player;
    private Animator animator;
    
    // Configurations
    [SerializeField] private float health = 100f;  
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float delayDeathTime = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float waitTime = 1f;
    public Transform sticker;
    
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
    
    
    //-Chaos
    //I'm changing the movement to path system
    //It should work better than what it is now
    //Will revert back it it fails
    //I'm gonna be writing another enemy script


    // Colliders
    private Collider2D boxCollider; // Never used but Collider helps in in-game Collision
    private Collider2D capsuleCollider; 

    // Rigibody
    private Rigidbody2D rb;
    
    void Start()
    {
        healthBarPrefab.SetMaxHealth(health);
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
        if(state == EnemyState.shooting) rb.velocity = Vector2.zero;
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
        if (state == EnemyState.death)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        distanceBetweenPlayerAndEnemy = Vector2.Distance(player.transform.position, transform.position);
        if (distanceBetweenPlayerAndEnemy < maxDistance)
        {
            rb.velocity = Vector2.zero;
            // Get the Player Direction
            float x = player.transform.position.x - transform.position.x;
            FlipSpriteOnPlayerSight(x);
            state = EnemyState.shooting;
            return;
        }
        
        // Check if enemy is still touching the ground
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
        {
            //print("not touching");
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
        if(!other.CompareTag($"Walls")) return;
        
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

    // For taking Damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBarPrefab.SetHealth(health);
        if (health <= 0)
        {
            for(var i=0;i<sticker.childCount;i++)
            {
                sticker.GetChild(i).GetComponent<Shuriken>().Rewind();
            }
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
}
