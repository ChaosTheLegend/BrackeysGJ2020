using System;
using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifespan;
    private Rigidbody2D rb;
    // private float direction;
    private Vector2 direction;
    private PlayerController player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        // direction = FindObjectOfType<EnemyController>().GetDirection();
        direction = (player.transform.position - transform.position).normalized * projectileSpeed; 
        // Vector2 changeInVelocity = new Vector2(direction * shurikenSpeed, 0f);
        Vector2 changeInVelocity = new Vector2(direction.x, direction.y);
        rb.velocity = changeInVelocity;
    }

    private void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // player.TakerDamage(damage);
        }
    }
}
