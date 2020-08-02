using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shuriken : MonoBehaviour
{
    private Vector2 initMousePos;
    private Vector2 mousePosWorld;

    [SerializeField] private float speed = 300f;

    [SerializeField] private float damageDealt = 10f;

    private Rigidbody2D rbody;

    private Vector2 moveDirection;

    [SerializeField] private int maxCollisions = 3;
    private int collisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Mouse's position during shuriken initialization
        initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosWorld = Camera.main.ScreenToWorldPoint(initMousePos);

        rbody = GetComponent<Rigidbody2D>();

        moveDirection = initMousePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();

        // Set initial velocity
        rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(collisionCount < maxCollisions)
        {
            // Maintain velocity in the speed the object is currently traveling in
            rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
        }
        else
        {
            rbody.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount = collisionCount < maxCollisions ? collisionCount + 1 : maxCollisions;
        Debug.Log(collisionCount);

        if (collision.gameObject.tag == "Enemy")
        {
#warning Chaos, this section needs the enemy's script name and damage method
            //EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
            //enemy.TakeDamage(damageDealt);
        }
    }
}
