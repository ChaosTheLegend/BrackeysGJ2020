using BrackeysGJ.MonoBehaviours;
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

    // used to store contact points for rewind
    [SerializeField] Vector2[] rewindPoints;

    private ShurikenThrow playerShurikenScript;

    private bool isRewinding = false;
    private int rewindIndex = 0;
    [SerializeField] private float fadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize collision points array
        rewindPoints = new Vector2[maxCollisions + 1];

        // Set initial rewind point
        rewindPoints[0] = transform.position;

        // Get a reference to the player
        playerShurikenScript = GameObject.FindObjectOfType<ShurikenThrow>();

        // Mouse's position during shuriken initialization
        initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosWorld = Camera.main.ScreenToWorldPoint(initMousePos);

        rbody = GetComponent<Rigidbody2D>();

        moveDirection = initMousePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();

        // Set initial velocity
        rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            if (collisionCount < maxCollisions)
            {
                // Maintain velocity in the speed the object is currently traveling in
                rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
            }
            else
            {
                rbody.velocity = Vector2.zero;
            }
        }
        else
        {
            RewindLogic();
            rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionCount < maxCollisions &&
            isRewinding == false)
        {
            rewindPoints[collisionCount + 1] = transform.position;

            collisionCount++;
            rewindIndex++;

            if (collision.gameObject.tag == "Enemy")
            {
#warning Chaos, blindspot's script needs a TakeDamage(float damageDealt) method
                //EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                //enemy.TakeDamage(damageDealt);
            }

            Debug.Log(collisionCount);
        }
    }

    public void Rewind()
    {
        isRewinding = true;
    }

    private void RewindLogic()
    {
        moveDirection = rewindPoints[rewindIndex] - new Vector2(transform.position.x, transform.position.y);
        rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;

        float distanceToRewindPoint = Vector2.Distance(transform.position, rewindPoints[rewindIndex]);
        if (distanceToRewindPoint <= 0.2)
        {
            rbody.velocity = Vector2.zero;

            // If we're not at our initial rewind point
            if (rewindIndex != 0)
            {
                // Move onto the next rewind point
                rewindIndex--;
            }
            // If we're at our initial rewind point
            else
            {
                // Our rewind is complete
                isRewinding = false;

                // Fade the shuriken out of existence
                StartCoroutine(FadeAway(fadeTime));
            }
            Debug.Log("DISTANCE REACHED");
        }

        IEnumerator FadeAway(float time)
        {
            Renderer renderer = transform.GetComponentInChildren<Renderer>();
            float alpha = renderer.material.color.a;
            for (float t = 0.0f; t < fadeTime; t += Time.deltaTime / time)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
                renderer.material.color = newColor;
                yield return null;
            }

            // Give the player the shuriken back
            playerShurikenScript.ReturnShuriken();

            // Destroy the shuriken
            Destroy(gameObject);
        }
    }
}
