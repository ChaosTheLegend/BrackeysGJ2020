using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private float damage = 20f;
    private Rigidbody2D rb;
    // private float direction;
    private Vector2 direction;
    private PlayerHealth player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        // direction = FindObjectOfType<EnemyController>().GetDirection();
        direction = (player.transform.position - transform.position) * projectileSpeed; 
        // Vector2 changeInVelocity = new Vector2(direction * shurikenSpeed, 0f);
        Vector2 changeInVelocity = new Vector2(direction.x, direction.y);
        rb.velocity = changeInVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
