using BrackeysGJ.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shuriken : MonoBehaviour
{
    private Vector2 initMousePos;
    private Vector2 mousePosWorld;

    [SerializeField] private float throwSpeed = 300f;
    [SerializeField] private float returnSpeed = 300f;


    [SerializeField] private float damageDealt = 10f;

    private Rigidbody2D rbody;

    private Collider2D collider;

    private Vector2 moveDirection;

    [SerializeField] private int maxCollisions = 3;
    private int collisionCount = 0;

    private ShurikenThrow playerShurikenScript;

    private bool isRewinding = false;

    [SerializeField] private float fadeAlpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the player
        playerShurikenScript = GameObject.FindObjectOfType<ShurikenThrow>();

        // Get a reference to the shuriken collider
        collider = GetComponent<Collider2D>();

        // Mouse's position during shuriken initialization
        initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Rigidbody2D
        rbody = GetComponent<Rigidbody2D>();

        // Direction for Shuriken to move
        moveDirection = initMousePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();

        // Set initial velocity
        rbody.velocity = moveDirection * throwSpeed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            if (collisionCount < maxCollisions)
            {
                // Maintain velocity in the speed the object is currently traveling in
                rbody.velocity = throwSpeed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
            }
            else
            {
                rbody.velocity = Vector2.zero;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, playerShurikenScript.transform.position, (returnSpeed / 25) * Time.deltaTime);
            //rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collisionCount < maxCollisions &&
                isRewinding == false)
            {
                collisionCount++;

                if (collision.gameObject.tag == "Enemy")
                {
                    #warning Chaos, blindspot's script needs a TakeDamage(float damageDealt) method
                    //EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                    //enemy.TakeDamage(damageDealt);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerShurikenScript.ReturnShuriken();
            Destroy(gameObject);
        }
    }

    public void Rewind()
    {
        // Tell code we're rewinding
        isRewinding = true;

        // Fade the shuriken's alpha
        Fade(fadeAlpha);

        // Make the collider a trigger so it can pass through walls
        collider.isTrigger = true;

        // Tell the shuriken to move towards the player
        //moveDirection = (Vector2)playerShurikenScript.transform.position - (Vector2)transform.position;
        //rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    private void Fade(float alphaValue)
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        renderer.material.color = new Color(1f, 1f, 1f, fadeAlpha);
    }

}
