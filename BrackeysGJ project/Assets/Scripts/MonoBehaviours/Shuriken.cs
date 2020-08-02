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

    private Rigidbody2D rbody;

    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        // Mouse's position during shuriken initialization
        initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosWorld = Camera.main.ScreenToWorldPoint(initMousePos);

        rbody = GetComponent<Rigidbody2D>();

        moveDirection = initMousePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();

        rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        rbody.velocity = speed * (rbody.velocity.normalized) * Time.fixedDeltaTime;
        //rbody.velocity = moveDirection * speed * Time.fixedDeltaTime;
    }
}
