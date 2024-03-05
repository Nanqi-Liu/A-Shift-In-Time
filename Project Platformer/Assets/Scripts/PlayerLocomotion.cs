using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputHandler inputHandler;

    [Header("Movement Stats")]
    [SerializeField]
    float moveSpeed = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<InputHandler>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        // HandleJump();
        // HandleFall();
    }

    private void HandleMovement()
    {
        Vector2 targetVelocity = Vector2.zero;
        targetVelocity.x = moveSpeed * inputHandler.movement;
        _rb.velocity = targetVelocity;
    }
}
