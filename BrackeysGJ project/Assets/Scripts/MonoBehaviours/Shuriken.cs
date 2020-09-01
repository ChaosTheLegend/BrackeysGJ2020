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
    private Vector2 velocity;
    private Animator _anim;

    [SerializeField] private float throwSpeed = 300f;
    [SerializeField] private float returnSpeed = 300f;
    [SerializeField] private float maxDistanceFromPlayer = 1f;

    [SerializeField] private float damageDealt = 35f;
    [SerializeField] private float damageDealtRewind = 35f;

    private List<EnemyController2> enemiesAlreadyDamaged = new List<EnemyController2>();

    private Rigidbody2D rbody;

    private Collider2D collider;

    private Vector2 moveDirection;

    [SerializeField] private int maxCollisions = 3;
    private int collisionCount = 0;

    private ShurikenThrow playerShurikenScript;
    private LineRenderer _line;

    private bool isRewinding = false;
    private bool Stuck = false;
    [SerializeField] private float fadeAlpha = 0.5f;

    private ShurikenSound sounds;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponentInChildren<ShurikenSound>();

        if (sounds == null)
        {
            Debug.LogError("Error: ShurikenSound.cs is missing on the " + gameObject.name + " gameObject.");
        }

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
        velocity = Vector2.one;

        //Line effect
        _line = GetComponent<LineRenderer>();
        //Animation
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        var currentVelocity = rbody.velocity;

        if (!isRewinding)
        {
            if (collisionCount < maxCollisions)
            {
                // Maintain velocity in the speed the object is currently traveling in
                rbody.velocity = (currentVelocity.normalized) * (throwSpeed * Time.fixedDeltaTime);
            }
            else
            {
                _anim.SetInteger($"State", 1);
            }
        }
        else
        {
            // rbody.simulated = false;
            // Dark - Needs to be simulated for collision detection during rewind
            // so that enemies can be damaged during rewind. Had to turn collider into trigger
            // instead so that it can still go through walls.
            rbody.simulated = true;
            collider.isTrigger = true;


            transform.position = Vector2.MoveTowards(transform.position, playerShurikenScript.transform.position, (returnSpeed) * Time.deltaTime);
            rbody.velocity = Vector2.zero;
            //Chaos has been here
            //and changed to smoothDamp instead of MoveTowards  
            //transform.position =  Vector2.SmoothDamp(transform.position, playerShurikenScript.transform.position, ref velocity,returnTime);
            if ((transform.position - playerShurikenScript.transform.position).sqrMagnitude < 0.7f * 0.7f)
            {
                sounds.StopRewindLoop();
                ReturnToPlayer();
            }
            else
            {
                sounds.PlayRewindLoop();
            }
        }

        // Rewind the shuriken if it gets too far away from the player
        if ((transform.position - playerShurikenScript.transform.position).sqrMagnitude >
            maxDistanceFromPlayer * maxDistanceFromPlayer && !Stuck) isRewinding = true;
    }

    private void Update()
    {
        // Show the rewind line if we're rewinding
        _line.enabled = isRewinding;
        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, playerShurikenScript.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount++;

        // If shuriken hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController2 enemy = collision.gameObject.GetComponent<EnemyController2>();

            // If this enemy wasn't already hit by this shuriken
            if (!enemiesAlreadyDamaged.Contains(enemy))
            {
                // Attach the shuriken to the enemy
                transform.SetParent(enemy.sticker, true);

                // Shuriken is now stuck
                StickShuriken();

                // Enemy takes damage
                enemy.TakeDamage(damageDealt);

                // This enemy has now taken damage from this shuriken
                enemiesAlreadyDamaged.Add(enemy);

            }
        }

        // Stops shuriken movement after collision
        if (collisionCount >= maxCollisions)
        {
            StickShuriken();
        }
        else
        {
            sounds.PlayBounce();
        }
    }

    // Collision detection during rewind
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRewinding)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyController2 enemy = collision.gameObject.GetComponent<EnemyController2>();

                // If this enemy hasn't already taken damage from this shuriken
                if (!enemiesAlreadyDamaged.Contains(enemy))
                {
                    // Enemy takes damage
                    enemy.TakeDamage(damageDealtRewind);

                    // This enemy has now taken damage from this shuriken
                    enemiesAlreadyDamaged.Add(enemy);
                }
            }
        }
    }

    /// <summary>
    /// Stops all shuriken movement
    /// </summary>
    private void StickShuriken()
    {
        collisionCount = maxCollisions;
        Stuck = true;
        rbody.velocity = Vector2.zero;
        rbody.simulated = false;
        sounds.PlayStuck();
    }

    /// <summary>
    /// Tell the shuriken to return to the players ammo
    /// </summary>
    private void ReturnToPlayer()
    {
        playerShurikenScript.ReturnShuriken(this);
        Destroy(gameObject);
    }

    public void Rewind()
    {
        // Tell code we're rewinding
        Stuck = false;
        isRewinding = true;

        //-Chaos
        //We will use animator for this
        _anim.SetInteger($"State", 2);
    }

    private void Fade(float alphaValue)
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        renderer.material.color = new Color(1f, 1f, 1f, alphaValue);
    }

}
