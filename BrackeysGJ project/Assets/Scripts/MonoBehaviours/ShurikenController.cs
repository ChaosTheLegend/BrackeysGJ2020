using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    [SerializeField] private float shurikenSpeed = 1f;
    private Rigidbody2D rb;
    private float direction;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = FindObjectOfType<EnemyController>().GetDirection();
    }

    void FixedUpdate()
    {
        Vector2 changeInVelocity = new Vector2(direction * shurikenSpeed, 0f);
        rb.velocity = changeInVelocity;
    }
}
