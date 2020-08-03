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


    [SerializeField] private float damageDealt = 10f;

    private Rigidbody2D rbody;

    private Collider2D collider;

    private Vector2 moveDirection;

    [SerializeField] private int maxCollisions = 3;
    private int collisionCount = 0;

    private ShurikenThrow playerShurikenScript;

    private bool isRewinding = false;
    private bool Stuck = false;
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
        velocity = Vector2.one;
        
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
                _anim.SetInteger($"State",1);
            }
        }
        else
        {
            transform.SetParent(null);
            transform.position = Vector2.MoveTowards(transform.position, playerShurikenScript.transform.position, (returnSpeed) * Time.deltaTime);
            rbody.velocity = Vector2.zero;
            //Chaos has been here
            //and changed to smoothDamp instead of MoveTowards  
            //transform.position =  Vector2.SmoothDamp(transform.position, playerShurikenScript.transform.position, ref velocity,returnTime);
            if((transform.position - playerShurikenScript.transform.position).sqrMagnitude < 0.7f*0.7f) ReturnToPlayer();
            //rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //-Chaos
        //What a hell is this?
        //Why not just disable shuriken-player collisions?
        //if (collision.gameObject.tag != "Player")
        //{
        if (isRewinding) return;
        if(Stuck) return;
        
        collisionCount++;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collisionCount = maxCollisions;
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            transform.SetParent(enemy.sticker,true);
            enemy.TakeDamage(damageDealt);
        }
        
        //-Chaos
        //This is done to stop shuriken immediately after collision
        //and prevent visual glitches
        //they still exist though :(
        if (collisionCount >= maxCollisions)
        {
            Stuck = true;
            rbody.velocity = Vector2.zero;
            rbody.simulated = false;
        }
            
            
        
        //}
    }

    
    //-Chaos
    //Since collision with player is disabled, this is useless
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerShurikenScript.ReturnShuriken();
            Destroy(gameObject);
        }
    }
    */
    //I moved it to another function
    private void ReturnToPlayer()
    {
        playerShurikenScript.ReturnShuriken();
        Destroy(gameObject);
    }

    public void Rewind()
    {
        // Tell code we're rewinding
        Stuck = true;
        isRewinding = true;

        //-Chaos
        //We will use animator for this
        _anim.SetInteger($"State",2);
        
        // Fade the shuriken's alpha
        
        //Fade(fadeAlpha);

        // Make the collider a trigger so it can pass through walls

        // Tell the shuriken to move towards the player
        //moveDirection = (Vector2)playerShurikenScript.transform.position - (Vector2)transform.position;
        //rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    private void Fade(float alphaValue)
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        renderer.material.color = new Color(1f, 1f, 1f, alphaValue);
    }

}
